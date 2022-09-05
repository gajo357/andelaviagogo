using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };
            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };
            PrepareAndSendEmails(customer, events);

            var customers = new List<Customer>{
                new Customer{ Name = "Nathan", City = "New York"},
                new Customer{ Name = "Bob", City = "Boston"},
                new Customer{ Name = "Cindy", City = "Chicago"},
                new Customer{ Name = "Lisa", City = "Los Angeles"}
            };
            foreach (var cust in customers)
            {
                PrepareAndSendEmails(cust, events);
            }

        }

        private static void PrepareAndSendEmails(Customer customer, List<Event> events)
        {
            // TODO: we could create a city match function that ignores the case, etc...
            var query = from result in events // loop all events
                        where result.City == customer.City  // find the ones from the same city
                        select result;
            // 1. TASK
            // for all events that match the city, we add them to the email
            Console.WriteLine("\nEvents from the same city");
            foreach (var item in query)
            {
                AddToEmail(customer, item);
            }
            /**
            We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */


            // 2. TASK
            // Get 5 closest matches and send them in an email
            var closestEvents = events
                .Select(e => new { Event = e, Distance = GetDistanceMemo(e.City, customer.City) })
                .Where(e => e.Distance.HasValue)
                .OrderBy(e => e.Distance)
                .Take(5);
            Console.WriteLine("\n5 closest events");
            foreach (var item in closestEvents)
            {
                AddToEmail(customer, item.Event);
            }

            // 5. TASK
            // sort by price
            Console.WriteLine("\nEvents sorted by price (cheapest first)");
            var cheapestEvents = events
                .Select(e => new { Event = e, Price = GetPrice(e) })
                .OrderBy(e => e.Price); // sort by price ascending
            foreach (var item in cheapestEvents)
            {
                Console.Write(item.Price.ToString("C"));
                Console.Write(" - ");
                AddToEmail(customer, item.Event);
            }
        }

        private static readonly IDictionary<string, int> Distances = new Dictionary<string, int>();
        static int? GetDistanceMemo(string fromCity, string toCity)
        {
            // no need to calculate anything if it is the same city
            if (fromCity == toCity)
                return 0;

            // 4. TASK
            // try-catch because the GetDistance can fail
            try
            {
                // 3. TASK
                // store distances for later use
                // create unique key so that we do not store the same distance twice
                var key = string.CompareOrdinal(fromCity, toCity) > 0 ? 
                    $"{fromCity} - {toCity}" : $"{toCity} - {fromCity}";
                if (!Distances.ContainsKey(key))
                {
                    // only calculate distance if it hasn't been already calculated
                    var value = GetDistance(fromCity, toCity);
                    Distances.Add(key, value);
                }

                return Distances[key];
            }
            catch (Exception e)
            {
                // log the message
                Console.WriteLine(e.Message);

                // TODO: ask the client and change this logic accordingly
                // return null to exclude this city
                return null;
            }
        }


        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i
            <
            Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
} 

/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/