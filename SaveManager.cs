using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameOfCells
{
    internal class SaveManager
    {
        public static SaveManager Instance { get; private set; } = new SaveManager();

        public HighScoreSave HighScoreSave { get; private set; } = HighScoreSave.Load();
    }

    public class HighScoreSave
    {
        public long Cells { get; set; } = 0;

        private const string SAVE_FILE = "user://highscore.json";

        [JsonIgnore]
        private bool saving = false;

        public static HighScoreSave Load()
        {

            if (File.Exists(SAVE_FILE))
            {
                var file = File.ReadAllText(SAVE_FILE);

                if (!string.IsNullOrEmpty(file))
                {
                    return JsonSerializer.Deserialize<HighScoreSave>(file);
                }
            }

            return new HighScoreSave();
        }

        /// <summary>
        /// Saves highscore data in a seperate thread.
        /// </summary>
        public void SaveAsync()
        {
            if (saving)
            {
                return;
            }

            Task.Run(() =>
            {
                saving = true;
                File.WriteAllText(SAVE_FILE, JsonSerializer.Serialize(this));
                saving = false;
            });
        }
    }
}
