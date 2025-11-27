using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TicketManagementSystem.API.Data;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    public class PdfReportService : BaseService, IPdfReportService
    {
        private readonly ApplicationDbContext _context;

        public PdfReportService(ApplicationDbContext context, ILogger<PdfReportService> logger) : base(logger)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<string> GenerateClosedTicketsReportAsync(DateTime from, DateTime to)
        {
            LogInformation("Starting generation of closed tickets report from {From} to {To}", from, to);

            var closedTickets = await GetClosedTicketsAsync(from, to);

            LogInformation("Found {Count} closed tickets for the report", closedTickets.Count);

            var fileName = $"ClosedTickets_{DateTime.Now:yyyyMMdd}.pdf";
            var filePath = Path.Combine("Reports", fileName);

            var document = CreateDocument(closedTickets, from, to);
            document.GeneratePdf(filePath);

            LogInformation("Closed tickets report generated successfully at {FilePath}", filePath);

            return filePath;
        }

        private async Task<List<ClosedTicketReportData>> GetClosedTicketsAsync(DateTime from, DateTime to)
        {
            var query = _context.Tickets
                .Where(t => t.Status == Status.Closed && t.CreatedAt >= from && t.CreatedAt <= to)
                .Join(_context.TicketHistories.Where(h => h.NewStatus == Status.Closed),
                    t => t.Id,
                    h => h.TicketId,
                    (t, h) => new { Ticket = t, History = h })
                .GroupBy(x => x.Ticket.Id)
                .Select(g => new
                {
                    Ticket = g.First().Ticket,
                    ClosedAt = g.Min(h => h.History.ChangedAt)
                })
                .Select(x => new ClosedTicketReportData
                {
                    Title = x.Ticket.Title,
                    Description = x.Ticket.Description,
                    CreatedAt = x.Ticket.CreatedAt,
                    ClosedAt = x.ClosedAt,
                    AssignedTo = x.Ticket.AssignedTo != null ? x.Ticket.AssignedTo.FullName : "Unassigned",
                    ResolutionTimeHours = (x.ClosedAt - x.Ticket.CreatedAt).TotalHours
                });

            return await query.ToListAsync();
        }

        private IDocument CreateDocument(List<ClosedTicketReportData> tickets, DateTime from, DateTime to)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Closed Tickets Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Report Period: {from:yyyy-MM-dd} to {to:yyyy-MM-dd}");
                            column.Item().Text($"Total Closed Tickets: {tickets.Count}");

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(100); // Title
                                    columns.RelativeColumn(); // Description
                                    columns.ConstantColumn(80); // Created At
                                    columns.ConstantColumn(80); // Closed At
                                    columns.ConstantColumn(100); // Assigned To
                                    columns.ConstantColumn(80); // Resolution Time
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(Block).Text("Title");
                                    header.Cell().Element(Block).Text("Description");
                                    header.Cell().Element(Block).Text("Created At");
                                    header.Cell().Element(Block).Text("Closed At");
                                    header.Cell().Element(Block).Text("Assigned To");
                                    header.Cell().Element(Block).Text("Resolution (hrs)");
                                });

                                foreach (var ticket in tickets)
                                {
                                    table.Cell().Element(Block).Text(ticket.Title);
                                    table.Cell().Element(Block).Text(ticket.Description.Length > 50 ? ticket.Description.Substring(0, 50) + "..." : ticket.Description);
                                    table.Cell().Element(Block).Text(ticket.CreatedAt.ToString("yyyy-MM-dd"));
                                    table.Cell().Element(Block).Text(ticket.ClosedAt.ToString("yyyy-MM-dd"));
                                    table.Cell().Element(Block).Text(ticket.AssignedTo);
                                    table.Cell().Element(Block).Text(ticket.ResolutionTimeHours.ToString("F2"));
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });
        }

        private IContainer Block(IContainer container)
        {
            return container
                .Border(1)
                .Background(Colors.Grey.Lighten3)
                .Padding(5);
        }
    }

    public class ClosedTicketReportData
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ClosedAt { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
        public double ResolutionTimeHours { get; set; }
    }
}