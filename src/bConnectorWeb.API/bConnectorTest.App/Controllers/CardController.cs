using System.Net.Http;
using System.Text;
using bConnectorTest.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Http;
using System;

public class CardController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseUrl;

    public CardController(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiBaseUrl = _configuration["ApiBaseUrl"];
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendDataToDatabase(CardModel model)
    {

        if (ModelState.IsValid)
        {

            var name = model.Name;
            var email = model.Email;
            var phone = model.Phone;
            var address = model.Address;



            // Your adaptive card JSON here (replace this with your actual adaptive card data)
            string adaptiveCardJson = $@"{{
        ""@type"": ""MessageCard"",
        ""@context"": ""http://schema.org/extensions"",
        ""summary"": ""Alert Summary"",
        ""sections"": [
            {{
                ""activityTitle"": ""From bConnector testing app"",
                ""activitySubtitle"": ""Testing: bConnector"",
                ""facts"": [
                    {{
                        ""name"": ""Name: "",
                        ""value"": ""{name}""
                    }},
                    {{
                        ""name"": ""Email: "",
                        ""value"": ""{email}""
                    }},
                    {{
                        ""name"": ""Phone: "",
                        ""value"": ""{phone}""
                    }},
                    {{
                        ""name"": ""Address: "",
                        ""value"": ""{address}""
                    }},
                    {{
                        ""name"": ""Action available:"",
                        ""value"": "" ""
                    }}
                ]
            }}
        ],
        ""potentialAction"": [
            {{
                ""@type"": ""OpenUri"",
                ""name"": ""youtube"",
                ""targets"": [
                    {{
                        ""os"": ""default"",
                        ""uri"": ""https://www.youtube.com/results?search_query=send+response+of+actionable+card+from+teams""
                    }}
                ]
            }},
            {{
                ""@type"": ""OpenUri"",
                ""name"": ""google"",
                ""targets"": [
                    {{
                        ""os"": ""default"",
                        ""uri"": ""https://www.youtube.com/results?search_query=send+response+of+actionable+card+from+teams""
                    }}
                ]
            }},
            {{
                ""@type"": ""ActionCard"",
                ""name"": ""select time"",
                ""inputs"": [
                    {{
                        ""@type"": ""MultichoiceInput"",
                        ""id"": ""pauseTime"",
                        ""title"": ""time111111"",
                        ""isMultiSelect"": false,
                        ""choices"": [
                            {{
                                ""display"": ""15"",
                                ""value"": ""15""
                            }},
                            {{
                                ""display"": ""30"",
                                ""value"": ""30""
                            }},
                            {{
                                ""display"": ""60"",
                                ""value"": ""60""
                            }},
                        ]
                    }}
                ],
                ""actions"": [
                    {{
                        ""@type"": ""HttpPOST"",
                        ""name"": ""Confirm"",
                        ""headers"": [
                            {{
                                ""name"": ""LmaoXD"",
                                ""value"": ""uwuw""
                            }}
                        ],
                        ""target"": ""https://f0f6-103-78-55-154.ngrok-free.app/api/Contacts/api/dueDate2"",
                        ""body"": ""{{\""duedate\"": \""{{{{pauseTime.value}}}}\"", \""alertData\"": \""false\""}}"",
                        ""bodyContentType"": ""application/json""
                    }}
                ]
            }}
        ]
    }}";

            string DatabaseData = $@"{{
        
            ""name"": ""{name}"",
            ""email"": ""{email}"",
            ""phone"": {phone},
            ""address"": ""{address}""
        
            }}";

            // Set the appropriate headers
            var content = new StringContent(adaptiveCardJson, Encoding.UTF8, "application/json");
            var content2 = new StringContent(DatabaseData, Encoding.UTF8, "application/json");

            // Replace "YOUR_WEBHOOK_URL" with the actual Teams incoming webhook URL
            string webhookUrl = "https://bizzntek.webhook.office.com/webhookb2/3a816351-c4a5-45be-9456-c225f4de4e53@f0238206-f8d2-4c0c-931f-acc6e68d3f58/IncomingWebhook/297d881f12c14845bd93a88c283c90bf/f3c9df23-48df-4d1f-98c0-cd67efaf88d2";




            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var response2 = await httpClient.PostAsync($"{_apiBaseUrl}/api/Contacts/add", content2);

                    // Log the response content for debugging
                    var responseContent = await response2.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response Content: " + responseContent);

                    if (response2.IsSuccessStatusCode)
                    {
                        // Both requests were successful
                        return Content("Adaptive card sent and data sent to database successfully.");
                    }

                    else
                    {
                        // Failed to send data to the database
                        return Content($"Failed to send data to the database. Status code: {response2.StatusCode}");
                    }
                }
                else
                {
                    // Failed to send the adaptive card
                    return Content($"Failed to send the adaptive card. Status code: {response.StatusCode}");
                }
            }

        }
        else
        {
            TempData["ResultMessage"] = "Invalid data entered.";
        }
        return RedirectToAction("Index");


    }

}
