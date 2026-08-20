using ClosedXML.Excel;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.ReportDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Oceanic_Horizon_Travel.Services.ReportServices
{
    public class ReportServices : IReportServices
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly ITourServices _tourServices;
        private readonly IMemberServices _memberServices;

        public ReportServices(
            IDatabaseSettings databaseSettings,
            ITourServices tourServices,
            IMemberServices memberServices)
        {
            _tourServices = tourServices;
            _memberServices = memberServices;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _bookingCollection = database.GetCollection<Booking>(databaseSettings.BookingCollectionName);
        }

        public async Task<ParticipantReportViewModel> GetReportAsync(string? tourDateId)
        {
            var model = new ParticipantReportViewModel
            {
                Tours = await _tourServices.GetAllAsync(),
                SelectedTourDateId = tourDateId
            };

            if (string.IsNullOrWhiteSpace(tourDateId)) return model;

            // İptal edilenler yolcu listesinde yer almaz
            var bookings = await _bookingCollection
                .Find(x => x.TourDateId == tourDateId && x.Status != "İptal Edildi")
                .SortBy(x => x.BookingDate)
                .ToListAsync();

            // Başlık bilgisi: seçilen kalkışı tur listesinden bul
            var tour = model.Tours.FirstOrDefault(t => t.TourDates.Any(d => d.Id == tourDateId));
            if (tour is not null)
            {
                model.TourTitle = tour.Title.Tr ?? "";
                var date = tour.TourDates.First(d => d.Id == tourDateId);
                model.StartDate = date.StartDate;
                model.EndDate = date.EndDate;
            }

            if (bookings.Count == 0) return model;

            // Üye adlarını toplu çek — N+1 önlemi
            var memberIds = bookings.Select(x => x.MemberId).Distinct().ToList();
            var members = await _memberServices.GetByIdsAsync(memberIds);
            var memberMap = members.ToDictionary(x => x.Id!, x => x);

            foreach (var booking in bookings)
            {
                var member = memberMap.GetValueOrDefault(booking.MemberId ?? "");
                var bookedBy = member is null ? "—" : $"{member.FirstName} {member.LastName}";
                var phone = member?.PhoneNumber ?? "";

                foreach (var guest in booking.Guests ?? new List<Entities.SubDocuments.Guest>())
                {
                    model.Participants.Add(new ParticipantDto
                    {
                        FullName = $"{guest.FirstName} {guest.LastName}",
                        IdentityNumber = guest.IdentityNumber ?? "",
                        BirthDate = guest.BirthDate,
                        BookingNumber = booking.BookingNumber ?? "",
                        BookedBy = bookedBy,
                        Phone = phone
                    });
                }
            }

            return model;
        }

        //EXCEL

        public async Task<byte[]> GenerateExcelAsync(string tourDateId)
        {
            var report = await GetReportAsync(tourDateId);

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Katılımcılar");

            // Başlık bloğu
            sheet.Cell(1, 1).Value = "KATILIMCI LİSTESİ";
            sheet.Range(1, 1, 1, 6).Merge();
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 14;

            sheet.Cell(2, 1).Value = report.TourTitle;
            sheet.Range(2, 1, 2, 6).Merge();

            sheet.Cell(3, 1).Value = report.StartDate.HasValue
                ? $"{report.StartDate:dd.MM.yyyy} — {report.EndDate:dd.MM.yyyy}"
                : "";
            sheet.Range(3, 1, 3, 6).Merge();

            sheet.Cell(4, 1).Value = $"Toplam katılımcı: {report.Participants.Count}";
            sheet.Range(4, 1, 4, 6).Merge();

            // Tablo başlıkları
            var headerRow = 6;
            string[] headers = { "#", "Ad Soyad", "Kimlik / Pasaport", "Doğum Tarihi", "Rezervasyon No", "Rezervasyonu Yapan" };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = sheet.Cell(headerRow, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0e2a3b");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Satırlar
            var row = headerRow + 1;
            var index = 1;

            foreach (var p in report.Participants)
            {
                sheet.Cell(row, 1).Value = index++;
                sheet.Cell(row, 2).Value = p.FullName;
                sheet.Cell(row, 3).Value = p.IdentityNumber;
                sheet.Cell(row, 4).Value = p.BirthDate.ToString("dd.MM.yyyy");
                sheet.Cell(row, 5).Value = p.BookingNumber;
                sheet.Cell(row, 6).Value = p.BookedBy;
                row++;
            }

            if (report.Participants.Count > 0)
            {
                sheet.Range(headerRow, 1, row - 1, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                sheet.Range(headerRow, 1, row - 1, 6).Style.Border.InsideBorder = XLBorderStyleValues.Hair;
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        // PDF 

        public async Task<byte[]> GeneratePdfAsync(string tourDateId)
        {
            var report = await GetReportAsync(tourDateId);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("KATILIMCI LİSTESİ")
                            .FontSize(16).Bold().FontColor("#0e2a3b");

                        column.Item().PaddingTop(4).Text(report.TourTitle)
                            .FontSize(11).FontColor("#0fa3a3");

                        if (report.StartDate.HasValue)
                        {
                            column.Item().Text($"{report.StartDate:dd.MM.yyyy} — {report.EndDate:dd.MM.yyyy}")
                                .FontSize(9).FontColor("#5d7180");
                        }

                        column.Item().PaddingTop(2).Text($"Toplam katılımcı: {report.Participants.Count}")
                            .FontSize(9).FontColor("#5d7180");

                        column.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e7e2d8");
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(24);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                        });

                        table.Header(header =>
                        {
                            static IContainer HeaderCell(IContainer c) =>
                                c.Background("#0e2a3b").Padding(5).DefaultTextStyle(t => t.FontColor("#ffffff").Bold());

                            header.Cell().Element(HeaderCell).Text("#");
                            header.Cell().Element(HeaderCell).Text("Ad Soyad");
                            header.Cell().Element(HeaderCell).Text("Kimlik / Pasaport");
                            header.Cell().Element(HeaderCell).Text("Doğum Tarihi");
                            header.Cell().Element(HeaderCell).Text("Rezervasyon No");
                        });

                        static IContainer BodyCell(IContainer c) =>
                            c.BorderBottom(1).BorderColor("#e7e2d8").Padding(5);

                        var index = 1;
                        foreach (var p in report.Participants)
                        {
                            table.Cell().Element(BodyCell).Text(index++.ToString());
                            table.Cell().Element(BodyCell).Text(p.FullName);
                            table.Cell().Element(BodyCell).Text(p.IdentityNumber);
                            table.Cell().Element(BodyCell).Text(p.BirthDate.ToString("dd.MM.yyyy"));
                            table.Cell().Element(BodyCell).Text(p.BookingNumber);
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(8).FontColor("#5d7180"));
                        text.Span("Oceanic Horizon Travel · ");
                        text.Span($"{DateTime.Now:dd.MM.yyyy HH:mm} · ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
