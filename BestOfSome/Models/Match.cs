namespace BestOfSome.Models ;

    public class Match
    {
        public string Winner;
        public Match Match1;
        public Match Match2;
        public readonly Match ParentMatch;
        public bool IsRightTurn = true;

        private Match(string item = null, Match c = null)
        {
            Winner = item;
            ParentMatch = c;
        }

        public Match(List<string> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private void Add(string item)
        {
            if (ParentMatch == null && Winner == null && Match1 == null && Match2 == null)
            {
                Winner = item;
                return;
            }
            if (Winner != null)
            {
                Match1 = new Match(Winner, this);
                Winner = null;
                Match2 = new Match(item, this);
                return;
            }
            if (IsRightTurn)
            {
                Match1.Add(item);
            }
            else
            {
                Match2.Add(item);
            }
            IsRightTurn = !IsRightTurn;
        }

        public void SetWinner(string c)
        {
            Winner = c;
        }

        public string Print()
        {
            if (Match1 == null)
            {
                return Winner;
            }
            var result1 = Match1.Print();
            var result2 = Match2.Print();
            return "(" + result1 + " VS " + result2 + ")";
        }

        private static int FindN(int x)
        {
            var i = 1;
            while (i < x)
            {
                i *= 2;
            }
            return i;
        }
    }