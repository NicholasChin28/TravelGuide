using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace TravelGuide.Tests.PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class UnitTest1 : PageTest
    {
        [Test]
        public async Task HasTitle()
        {
            await Page.GotoAsync("https://playwright.dev");

            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
        }

        [Test]
        public async Task DetectRoomLinks()
        {
            await Page.GotoAsync("https://www.airbnb.com.my/s/Japan/homes?tab_id=home_tab&refinement_paths[]=%2Fhomes&flexible_trip_lengths[]=one_week&monthly_start_date=2024-10-01&monthly_length=3&monthly_end_date=2025-01-01&price_filter_input_type=0&channel=EXPLORE&place_id=ChIJLxl_1w9OZzQRRFJmfNR1QvU&date_picker_type=calendar&checkin=2024-09-05&checkout=2024-09-12&source=structured_search_input_header&search_type=filter_change&_set_bev_on_new_domain=1725336796_EAZGVmY2NmNTg4OW");

            await Page.WaitForSelectorAsync("div[itemprop='itemListElement']");

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var roomLinks = await Page.EvaluateAsync<string[]>(@"
                Array.from(document.querySelectorAll('div[itemprop=""itemListElement""]'))
                    .map(div => div.querySelector('div > div > div > a'))
                    .filter(a => a && a.href && a.href.includes('/rooms/'))
                    .map(a => a.href)
            ");

            Console.WriteLine("Total count: " + roomLinks.Count());

            Console.WriteLine("Detected room links:");
            foreach (var link in roomLinks)
            {
                Console.WriteLine(link);
            }

            Assert.That(roomLinks.Length, Is.GreaterThan(0), "No room links were found on the page.");

            foreach (var link in roomLinks)
            {
                Assert.That(link, Does.Match(@"/rooms/\d+"),
                    $"Link does not match expected pattern: {link}");
            }
        }
    }
}