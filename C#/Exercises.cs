using System;
using System.Linq;
using System.Reflection.Emit;
using SampleQueries;
using SampleSupport;

namespace QuerySamples
{
    [Title("208323")]
    [Prefix("Linq")]
    internal class Exercises : LinqSamples
    {
        [Category("Lab1")]
        [Title("1")]
        [Description("nazwy i ceny jednostkowe wszystkich produktów")]
        public void Linq1P1()
        {
            var products = GetProductList();

            foreach (var x in from num in products
                select new {num.ProductName, num.UnitPrice})
                Console.WriteLine(x);
        }

        [Category("Lab1")]
        [Title("2")]
        [Description("nazwy produktów, które są na stanie, kosztują mniej niż 10 i należą do kategorii Seafood")]
        public void Linq1P2()
        {
            var products = GetProductList();

            foreach (var x in from product in products
                where (product.Category == "Seafood") && (product.UnitPrice < 10)
                select new {product.ProductName, product.UnitPrice})
                Console.WriteLine(x);
        }


        [Category("Lab1")]
        [Title("3")]
        [Description("produkty, których cena jednostkowa, jest równa cenie produktu o nazwie Ikura. ")]
        public void Linq1P3()
        {
            var products = GetProductList();

            var pro = from product1 in products where product1.ProductName == "Ikura" select product1.UnitPrice;
            foreach (var x in from product in products
                where pro.Contains(product.UnitPrice)
                select new {product.ProductName, product.UnitPrice})
                Console.WriteLine(x);
        }


        [Category("Lab1")]
        [Title("4")]
        [Description("średnią cenę produktów w każdej kategorii (użyj group by). ")]
        public void Linq1P4()
        {
            var products = GetProductList();

            foreach (var x in from product in products
                group product by product.Category
                into gr
                select new {avg = gr.Average(p => p.UnitPrice), name = gr})
                Console.WriteLine(x);
        }

        [Category("Lab1")]
        [Title("3.1")]
        [Description(
             "* W dziale LINQ to Objects napisz zapytanie zwracające liczby pierwsze z zakresu od 1 do 888. (WSKAZÓWKA: użyj metody Enumerable.Range(...)) "
         )]
        public void Linq1P31()
        {
            var result = Enumerable.Range(3, 888).Where(y => Enumerable.Range(2, y - 1).All(x => y%x != 0.0));

            foreach (var variable in result)
            {
                Console.WriteLine(variable);
            }
        }

        [Category("Lab1")]
        [Title("3.2")]
        [Description("średnią cenę produktów w każdej kategorii (użyj group by). ")]
        public void Linq1P32()
        {
            var products = GetProductList();

            var result = from product in products
                group product by product.Category
                into gr
                select new {avg = gr.Average(p => p.UnitPrice), name = gr};

            Console.WriteLine(Stopwatch.TestTime(result, 10));
            Console.WriteLine(Stopwatch.TestTime(result, 20));
            Console.WriteLine(Stopwatch.TestTime(result, 30));
        }



        /// <summary>
        // Lab 2  /////////////////////////////
        /// </summary>
        
        [Category("Lab2")]
        [Title("3.2 1")]
        [Description("Znaleźć nazwy produktów, które są na stanie, kosztują mniej niż 10 i należą do kategorii Seafood."
         )]
        public void Linq2P321()
        {
            var products = GetProductList();

            const int none = 0;
            const int x2 = 10;
            const string category1 = "Seafood";

            var result =
                products.Where(p => (p.UnitPrice < x2) && p.Category.Equals(category1) && (p.UnitsInStock != none))
                    .Select(p => p.ProductName);
            Console.WriteLine("V1 " + Stopwatch.TestTime(result, 10));
            Console.WriteLine("V1 Parallel " + Stopwatch.TestTime(result.AsParallel(), 10));
            //V1 			0,1981 ms
            //V1 Parallel   0,1952 ms

            var result2 =
                products.Where(p => (p.UnitPrice < 10) && p.Category.Equals(category1) && (p.UnitsInStock != 0))
                    .Select(p => p.ProductName);
            Console.WriteLine("V2 " + Stopwatch.TestTime(result2, 10));
            Console.WriteLine("V2 Parallel " + Stopwatch.TestTime(result2.AsParallel(), 10));
            //V2 			0,1948 ms
            //V2 Parallel   0,1981 ms
        }

        [Category("Lab2")]
        [Title("3.2 2")]
        [Description(
             "Dla każdej kategorii znaleźć najtańsze i najdroższe produkty. Zwrócić nazwy kategorii i nazwy produktów. * Napisz zapytanie o złożoności lepszej niż O(k * log(k) * n * n), gdzie n odnosi się do liczby produktów, a k do kategorii."
         )]
        public void LinqwP322()
        {
            var products = GetProductList();

            var result1 =
                products.GroupBy(x => x.Category, x => new {x.ProductName, x.UnitPrice}).Select(
                    category => category.Where(product =>
                            (product.UnitPrice == category.Min(p => p.UnitPrice)) ||
                            (product.UnitPrice == category.Max(p => p.UnitPrice)))
                        .Select(x => x.ProductName));
            
            var result1P =
                products.AsParallel().GroupBy(x => x.Category, x => new {x.ProductName, x.UnitPrice}).Select(
                    category => category.Where(product =>
                            (product.UnitPrice == category.Min(p => p.UnitPrice)) ||
                            (product.UnitPrice == category.Max(p => p.UnitPrice)))
                        .Select(x => x.ProductName));
            Console.WriteLine("V1 " + Stopwatch.TestTime(result1, 10));
            Console.WriteLine("V1 Parallel " + Stopwatch.TestTime(result1P, 10));
            //V1 			1,202  ms
            //V1 Parallel   2,0654 ms

                                    var result2 =
                products.GroupBy(x => x.Category, x => new {x.ProductName, x.UnitPrice})
                    .Select(c => new {c, Min = c.Min(p => p.UnitPrice), Max = c.Max(p => p.UnitPrice)})
                    .Select(
                        y =>
                            y.c.Where(q => (q.UnitPrice == y.Min) || (q.UnitPrice == y.Max))
                                .Select(b => new {b.ProductName, y.c.Key}));

            var result2P =
                products.AsParallel().GroupBy(x => x.Category).AsParallel()
                    .Select(c => new {c, Min = c.Min(p => p.UnitPrice), Max = c.Max(p => p.UnitPrice)})
                    .Select(
                        y =>
                            y.c.Where(q => (q.UnitPrice == y.Min) || (q.UnitPrice == y.Max))
                                .Select(b => new {b.ProductName, y.c.Key}));
            Console.WriteLine("V2 " + Stopwatch.TestTime(result2, 10));
            Console.WriteLine("V2 Parallel " + Stopwatch.TestTime(result2P, 10));
            //V2 			1,6704 ms
            //V2 Parallel   3,0734 ms

        }

        [Category("Lab2")]
        [Title("3.2 3")]
        [Description(
             "Znaleźć cenę, dla której jest najwięcej sztuk produktów (biorąc pod uwagę też unitInStocks). Zwrócić cenę i liczbę sztuk."
         )]
        public void Linq2P323()
        {
            var products = GetProductList();

            //var result1 =
            //    products.GroupBy(x => x.UnitPrice)
            //        .Select(a => new {Price = a.Key, Count = a.Sum(y => y.UnitsInStock)})
            //        .Aggregate((a, b) => a.Count > b.Count ? a : b).Price;

            //Console.WriteLine(Stopwatch.TestTime(result1, 10));

            var sum1 =
                products.GroupBy(a => a.UnitPrice, a => a.UnitsInStock)
                    .Select(a => new {key = a.Key, suma = a.Sum(b => b)});
            var result1 = sum1.Where(x => x.suma == sum1.Max(z => z.suma)).Select(x => new {x.suma, x.key});
            
            var sum1P =
               products.AsParallel().GroupBy(a => a.UnitPrice, a => a.UnitsInStock)
                   .Select(a => new { key = a.Key, suma = a.Sum(b => b) });
            var result1P = sum1P.Where(x => x.suma == sum1P.Max(z => z.suma)).Select(x => new { x.suma, x.key });
            Console.WriteLine(Stopwatch.TestTime(result1, 2));
            Console.WriteLine(Stopwatch.TestTime(result1P, 2));
            //V1 			31051,4565 ms
            //V1 Parallel   29684,3327 ms
        }

        [Category("Lab2")]
        [Title("3.2 4")]
        [Description(
             "Dla każdego produktu podać liczbę produktów, które są od niego tańsze lub jest ich mniej sztuk na składzie."
         )]
        public void Linq2P324()
        {
            var products = GetProductList();

            var result1 =
                products.Select(p => products.Count(e => (e.UnitPrice < p.UnitPrice) || (e.UnitsInStock < p.UnitPrice)));
            
            var result2 =
                products.AsParallel()
                    .Select(p => products.Count(e => (e.UnitPrice < p.UnitPrice) || (e.UnitsInStock < p.UnitPrice)));
            Console.WriteLine(Stopwatch.TestTime(result1, 10));
            Console.WriteLine(Stopwatch.TestTime(result2, 10));
            //V1 			4687,5516 ms
            //V1 Parallel   1877,3146 ms
        }

        [Category("Lab2")]
        [Title("3.2 5")]
        [Description(
             "Dla każdego produktu podaj liczbę produktów, które kosztują tyle samo. *Napisz zapytanie o złożoności O(n * log(n))."
         )]
        public void Linq2P325()
        {
            var products = GetProductList();
            var TOLERANCE = 0.0001;

            var result1 = products.Select(p1 => products.Count(p2 => Math.Abs(p2.UnitPrice - p1.UnitPrice) < TOLERANCE));
            
            var result1P =
                products.AsParallel()
                    .Select(p1 => products.Count(p2 => Math.Abs(p2.UnitPrice - p1.UnitPrice) < TOLERANCE));
            Console.WriteLine(Stopwatch.TestTime(result1, 10));
            Console.WriteLine(Stopwatch.TestTime(result1P, 10));
            //V1			3128,0204 ms
            //V1 Parallel   2010,2876 ms
        }

        [Category("Lab2")]
        [Title("3.2 7")]
        [Description(
             "Sprawdź wydajność zapytania z wykładu i poprzedzających laboratoriów: produkty, których cena jednostkowa, jest równa cenie produktu o nazwie Ikura. Spróbuj ją poprawić."
         )]
        public void Linq2P327()
        {
            var products = GetProductList();

            //Rozwiązanie z Lab 1
            //var pro = from product1 in products where product1.ProductName == "Ikura" select product1.UnitPrice;
            //foreach (var x in from product in products
            //                  where pro.Contains(product.UnitPrice)
            //                  select new { product.ProductName, product.UnitPrice })
            //    Console.WriteLine(x);

            var IKURA = "IKURA";
            var result1 = products.Where(x => x.ProductName.Equals(IKURA)).Select(x => x.UnitPrice);
            var result11 = products.Where(x => result1.Contains(x.UnitPrice));
            
            var result1P = products.AsParallel().Where(x => x.ProductName.Equals(IKURA)).Select(x => x.UnitPrice);
            var result11P = products.Where(x => result1P.Contains(x.UnitPrice));
            Console.WriteLine(Stopwatch.TestTime(result11, 2));
            Console.WriteLine(Stopwatch.TestTime(result11P, 2));
            //V1			2185,8099 ms
            //V1 Parallel   4607,8077 ms
        }
    }
}