using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestOfSome.Models;

namespace BestOfSome ;

    public partial class MatchPage : ContentPage
    {
        private Dictionary<int, List<Match>> dictionary;
        private int allCount = 0;
        private int currentCount = 0;
        private Label label;
        
        public MatchPage(Dictionary<int, List<Match>> dict ,int num)
        {
            InitializeComponent();
            dictionary = dict;
            allCount = dictionary[num].Count;
            var stackLayout = new StackLayout{};
            foreach (var item in dictionary[num])
            {
                stackLayout.Children.Add(NSL(item));
            }
            

            var currentLabel = new Label
            {
                Text = currentCount.ToString()
            };
            label = currentLabel;

            var allLabel = new Label
            {
                Text = allCount.ToString()
            };
            
            var horizontalSl = new HorizontalStackLayout
            {
                Margin = 20,
                Children = { currentLabel, new Label{Text = "/"}, allLabel}
            };
            
            var button = new Button
            {
                Text = "Next Page",
                Background = Colors.White,
                TextColor = Colors.Black
            };
            button.Clicked += async (sender, e) =>
            {
                if (currentCount < allCount)
                {
                    await DisplayAlert("Error", "Some of the matches still remained!", "OK");
                }
                else
                {
                    if (num == 0)
                    {
                        await DisplayAlert("Congratulation", $"the winner is {dictionary[0][0].Winner}", "OK");
                    }
                    else
                    {
                        await Navigation.PushAsync(new MatchPage(dictionary, num-1));
                    }
                }
            };
            stackLayout.Children.Add(horizontalSl);
            stackLayout.Children.Add(button);
            var sv = new ScrollView
            {
                Content = stackLayout
            };
            
            Content = sv;
        }

        public StackLayout NSL(Match match)
        {
            var firstButton = new Button
            {
                BackgroundColor = Colors.Aqua,
                TextColor = Colors.Black,
                Text = ""
                
            };
            
            var winnerButton = new Button
            {
                BackgroundColor = Colors.Aqua,
                TextColor = Colors.Black,
                Text = ""
            };

            var secondButton = new Button
            {
                BackgroundColor = Colors.Aqua,
                TextColor = Colors.Black,
                Text = ""
            };
            
            if (match.Match1 == null)
            {
                currentCount++;
                firstButton.Text = match.Winner;
                firstButton.BackgroundColor = Colors.Green;
                winnerButton.Text = match.Winner;
                winnerButton.BackgroundColor = Colors.Aqua;
            }
            else
            {
                firstButton.Text = match.Match1.Winner;
                secondButton.Text = match.Match2.Winner;
                
                firstButton.Clicked += (sender, e) =>
                {
                    if (winnerButton.Text == "")
                    {
                        currentCount++;
                        label.Text = currentCount.ToString();
                    }
                    secondButton.BackgroundColor = Colors.Red;
                    firstButton.BackgroundColor = Colors.Green;
                    var value = firstButton.Text;
                    match.Winner = value;
                    winnerButton.Text = value;
                };
                secondButton.Clicked += (sender, e) =>
                {
                    if (winnerButton.Text == "")
                    {
                        currentCount++;
                        label.Text = currentCount.ToString();
                    }
                    firstButton.BackgroundColor = Colors.Red;
                    secondButton.BackgroundColor = Colors.Green;
                    var value = secondButton.Text;
                    match.Winner = value;
                    winnerButton.Text = value;
                };
            }

            var stackLayout = new StackLayout
            {
                BackgroundColor = Colors.White,
                Margin = 20,
                Spacing = 5,
                Children = { firstButton, winnerButton, secondButton }
            };

            return stackLayout;
        }
    }