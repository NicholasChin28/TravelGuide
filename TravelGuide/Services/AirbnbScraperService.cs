using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using TravelGuide.DataTransferObjects.Accomodations.Base;
using TravelGuide.Services.Interfaces;

namespace TravelGuide.Services
{
    public class AirbnbScraperService : IAirbnbScraperService
    {
        private readonly IBrowserContext browserContext;

        public AirbnbScraperService(IBrowserContext browserContext)
        {
            this.browserContext = browserContext;
        }

        public async Task<IEnumerable<BaseHotelDto>> ScrapeHotelAsync(int priceMin, int priceMax)
        {
            var page = await browserContext.NewPageAsync();
            //using var playwright = await Playwright.CreateAsync();
            //await using var browser = await playwright.Firefox.LaunchAsync();
            //await using var context = await browser.NewContextAsync();

            //var page = await context.NewPageAsync();

            var url = $"https://www.airbnb.com/s/Japan/homes?tab_id=home_tab&refinement_paths%5B%5D=%2Fhomes&flexible_trip_lengths%5B%5D=one_week&monthly_start_date=2024-10-01&monthly_length=3&monthly_end_date=2025-01-01&channel=EXPLORE&place_id=ChIJLxl_1w9OZzQRRFJmfNR1QvU&date_picker_type=calendar&checkin=2024-09-05&checkout=2024-09-21&source=structured_search_input_header&search_type=filter_change&query=Japan&search_mode=regular_search&price_filter_num_nights=16&price_min={priceMin}&price_max={priceMax}";

            await page.GotoAsync(url);
            await page.WaitForSelectorAsync("div[itemprop='itemListElement']");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var roomLinks = await page.EvaluateAsync<string[]>(@"
                Array.from(document.querySelectorAll('div[itemprop=""itemListElement""]'))
                    .map(div => div.querySelector('div > div > div > a'))
                    .filter(a => a && a.href && a.href.includes('/rooms/'))
                    .map(a => a.href)
            ");

            Console.WriteLine($"Total rooms found: {roomLinks.Length}");

            var hotels = new List<BaseHotelDto>();

            foreach (var link in roomLinks)
            {
                // Here you would typically navigate to each link and scrape the details
                // For demonstration, we're just creating a basic DTO with the link
                hotels.Add(new BaseHotelDto
                {
                    //Url = link,
                    // Add other properties as needed
                });
            }

            await page.CloseAsync();

            return hotels;
        }
    }
}