using BestOfSome.Models;

namespace BestOfSome ;

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            var selectFileButton = new Button
            {
                Text = "Select File"
            };
            // Create the entry
            var editor = new Editor
            {
                Placeholder = "Enter text here",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20),
                AutoSize = EditorAutoSizeOption.TextChanges // This makes the Editor adjust its height based on the text
            };

            // Create the button
            var button = new Button
            {
                Text = "Submit",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            // Button click event
            button.Clicked += (sender, e) =>
            {
                // Logic to handle the entry value on button click
                var enteredText = editor.Text; // This retrieves the text from the entry
                // For example, you can display it in a dialog or use it for other purposes
                var items = ReadFromString(enteredText);
                GoToNextPage(items);
            };
            

            selectFileButton.Clicked += async (sender, e) =>
            {
                try
                {
                    var result = await FilePicker.PickAsync(new PickOptions
                    {
                        PickerTitle = "Select a file"
                    });

                    if (result == null) return;
                    var selectedFilePath = result.FullPath;
                    var items = Reader(selectedFilePath);
                    GoToNextPage(items);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error selecting file: {ex.Message}", "OK");
                }
            };

            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = { selectFileButton , editor, button}
            };
        }

        public void GoToNextPage(List<string> items)
        {
            var n = FindN(items.Count);
            Shuffle(items);
            var match = new Match(items);
            var dictionary = new Dictionary<int, List<Match>> { { 0, new List<Match>() } };
            dictionary[0].Add(match);
            for (var i = 1; i < n; i++)
            {
                dictionary.Add(i, new List<Match>());
                foreach (var matchItem in dictionary[i-1].Where(matchItem => matchItem.Match1 != null))
                {
                    dictionary[i].Add(matchItem.Match1);
                    dictionary[i].Add(matchItem.Match2);
                }
            }
            Application.Current.MainPage = new NavigationPage(new MatchPage(dictionary, n-1));
        }
        
        static void Shuffle<T>(List<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    
        private static int FindN(int x)
        {
            var i = 1;
            var j = 0;
            while (i < x)
            {
                i *= 2;
                j++;
            }
            return j;
        }

        private static List<string> Reader(string path)
        {
            var items = new List<string>();

            using var sr = new StreamReader(path);
            try
            {
                while (sr.ReadLine() is {}line)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        items.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the file: " + e.Message);
            }
            return items;
        }
        
        private static List<string> ReadFromString(string input)
        {
            var items = new List<string>();

            using var sr = new StringReader(input);
            try
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        items.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the string: " + e.Message);
            }

            return items;
        }

    }