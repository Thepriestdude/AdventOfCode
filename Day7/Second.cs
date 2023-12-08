namespace Day7;

public sealed class Second
{
    public static int Do(string[] input)
    {
        var hands = input.Select(ParseHand).ToList();
        hands.Sort();
        return hands.Select((hand, i) => hand.Bid * (i + 1)).Sum();
    }

    private static Hand ParseHand(string input)
    {
        var rawHand = input.Split(" ");
        return new Hand(rawHand[0].Select(x => new Card(x)).ToList(), int.Parse(rawHand[1]));
    }

    record Card
    {
        public int Value { get; }

        public Card(char value)
        {
            Value = value switch
            {
                'J' => Value = 1,
                'T' => Value = 10,
                'Q' => Value = 12,
                'K' => Value = 13,
                'A' => Value = 14,
                _ => Value = int.Parse(value.ToString())
            };
        }
    }

    class Hand(List<Card> cards, int bid) : IComparable<Hand>
    {
        public List<Card> cards { get; set; } = cards;
        public int Bid { get; } = bid;

        public Rank GetRank()
        {
            var cardCount = cards.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Count());
            return cardCount switch
            {
                _ when cardCount.Values.Any(x => x == 5) => Rank.FiveOfAKind, // done
                
                var x when cardCount.Values.Any(x => x == 3) && cardCount.Values.Any(x => x == 2) => // done
                    x.TryGetValue(1, out _) 
                        ? Rank.FiveOfAKind 
                        : Rank.FullHouse,
                
                var x when cardCount.Values.Any(x => x == 4) => // done
                    x.TryGetValue(1, out _) 
                        ? Rank.FiveOfAKind 
                        : Rank.FourOfAKind,
                
                var x when cardCount.Values.Any(x => x == 3) => // done
                    x.TryGetValue(1, out _) 
                        ? Rank.FourOfAKind 
                        : Rank.ThreeOfAKind,
                
                var x when cardCount.Values.Any(x => x == 2) && cardCount.Count == 3 =>  // done
                    x.TryGetValue(1, out var jCount) 
                        ? jCount == 2 
                            ? Rank.FourOfAKind 
                            : Rank.FullHouse 
                        : Rank.TwoPairs,
                
                var x when cardCount.Values.Any(x => x == 2) => 
                    x.TryGetValue(1, out var jCount) 
                        ? Rank.ThreeOfAKind 
                        : Rank.OnePair,
                
                var x => x.TryGetValue(1, out var jCount) ? Rank.OnePair : Rank.HighCard
            };
        }


        public int CompareTo(Hand? other)
        {
            if (other == null) return 1;
            var rank = GetRank();
            var otherRank = other.GetRank();
            if (rank > otherRank) return 1;
            if (rank < otherRank) return -1;
            if (rank == otherRank)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (cards[i].Value > other.cards[i].Value) return 1;
                    if (cards[i].Value < other.cards[i].Value) return -1;
                }
            }

            return 0;
        }
    }

    enum Rank
    {
        HighCard = 0,
        OnePair = 1,
        TwoPairs = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6,
    }
}