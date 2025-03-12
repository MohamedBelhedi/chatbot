using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatbotApi.Services
{
    public class GetKnbResponse
    {
        public async Task<string> ReadFileAsync()
        {
            try
            {
                using StreamReader reader = new StreamReader("./Assets/knb/openai_kbn.txt");
                Console.WriteLine(reader.ToString());
                return await reader.ReadToEndAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine("Die Datei konnte nicht gelesen werden:");
                Console.WriteLine(e.Message);
                return string.Empty; // 🔥 Rückgabe eines leeren Strings anstatt void!
            }
        }
    }
}
