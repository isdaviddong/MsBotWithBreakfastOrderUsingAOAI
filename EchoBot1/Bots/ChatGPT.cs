using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace EchoBot1.Bots
{
    public class ChatGPT
    {
        const string AzureOpenAIEndpoint = "https://________.openai.azure.com";  //👉replace it with your Azure OpenAI Endpoint
        const string AzureOpenAIModelName = "__________"; //👉repleace it with your Azure OpenAI Model Name
        const string AzureOpenAIToken = "______________________"; //👉repleace it with your Azure OpenAI Token
        const string AzureOpenAIVersion = "2024-02-15-preview";  //👉replace  it with your Azure OpenAI Model Version

        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum role
        {
            assistant, user, system
        }

        public static string CallAzureOpenAIChatAPI(
            string endpoint, string modelName, string apiKey, string apiVersion, object requestData)
        {
            var client = new HttpClient();

            // 設定 API 網址
            var apiUrl = $"{endpoint}/openai/deployments/{modelName}/chat/completions?api-version={apiVersion}";

            // 設定 HTTP request headers
            client.DefaultRequestHeaders.Add("api-key", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT heade
            // 將 requestData 物件序列化成 JSON 字串
            string jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            // 建立 HTTP request 內容
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
            // 傳送 HTTP POST request
            var response = client.PostAsync(apiUrl, content).Result;
            // 取得 HTTP response 內容
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
            return obj.choices[0].message.content.Value;
        }


        public static string getResponseFromGPT(string Message)
        {
            return ChatGPT.CallAzureOpenAIChatAPI(
               AzureOpenAIEndpoint, AzureOpenAIModelName, AzureOpenAIToken, AzureOpenAIVersion,
                //ref: https://learn.microsoft.com/en-us/azure/cognitive-services/openai/reference#chat-completions
                new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        //system prompt
                        new {
                            role = ChatGPT.role.system ,
                            content = @"
                            你是餐飲店的客服機器人，會回答客戶詢問的餐點問題，並且推薦讓店裡獲利最高的產品。你也能處裡客戶的抱怨和申訴，安撫客戶情緒。
                            目前店裡面所販賣的產品有底下這些:
                            大亨堡，45元，豬肉
                            麥香雞，36元，雞肉
                            蛋餅，27元，只有蛋，是蛋奶素
                            可麗餅，50元，是蛋奶素
                            飯糰，45元，有肉鬆，非素食


                            而飲料有底下這些:
                            可樂，55元
                            紅茶，35元
                            奶茶，45元

                            店裡沒有套餐，只能把餐點和飲料搭配銷售，這樣搭配沒有折扣。此外，跟客人推薦餐點時，必須是上述的清單中的餐點。
                            目前唯一的優惠是周日早上買一送一，只限單點大亨堡才有這個優惠。
                            ----------------------
                            "
                        },
                        //history
                          new {
                             role = ChatGPT.role.user,
                             content = "請告訴我最便宜的餐點價格。"
                        },
                            new {
                             role = ChatGPT.role.assistant,
                             content = "你可以點選蛋餅，只需要27元"
                        },

                                new {
                             role = ChatGPT.role.user,
                             content = "有什麼好吃的餐點嗎?"
                        },
                            new {
                             role = ChatGPT.role.assistant,
                             content = @"這個問題很主觀，不同的人有不同的口味喜好，以下是我個人的推薦:
1. 大亨堡：這是我們餐廳的招牌餐點，肉質鮮美，搭配特製的醬料非常美味
2. 麥香雞：這是一個很受歡迎的餐點，雞肉鮮嫩，外皮酥脆，非常適合當作午餐或晚餐 
3. 可麗餅：這是一個很受歡迎的早餐餐點，口感軟嫩，搭配楓糖漿和奶油非常美味"
                        },


                                new {
                             role = ChatGPT.role.user,
                             content = "我想要吃飽一點，你可以推薦我豐盛的套餐嗎?"
                        },
                            new {
                             role = ChatGPT.role.assistant,
                             content = "很抱歉，我們店裡沒有套餐，只能把餐點和飲料搭配銷售。\r\n但是，如果你想吃得飽一點，我建議你可以考慮以下搭配：\r\n1. 大亨堡 + 可樂：肉質鮮美，搭配可樂口感更佳。\r\n2. 麥香雞 + 紅茶：雞肉鮮嫩，搭配紅茶口感更清爽。\r\n3. 飯糰 + 奶茶：飯糰營養豐富，搭配奶茶口感更香濃。\r\n希望這些搭配可以滿足你的食慾需求！"
                        },


                                     new {
                             role = ChatGPT.role.user,
                             content = "周日早上，用100元可以買到幾個大亨堡?"
                        },
                            new {
                             role = ChatGPT.role.assistant,
                             content = "因為100元可以買兩個，但周一有買一送一的優惠，所以是四個，而不是兩個。"
                        },

                        //user message/prompt
                        new {
                             role = ChatGPT.role.user,
                             content = Message
                        },
                    }
                });
        }
    }

}
