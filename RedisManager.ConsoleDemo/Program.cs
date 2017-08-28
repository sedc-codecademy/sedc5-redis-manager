using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisManager.ConsoleDemo
{
    class Program
    {
        public const string ConnectionString = "sedc-redis-cache.redis.cache.windows.net:6380,password=lxakE7VAVpdCQSmb0JFwhl7lXg2C07bj68XLyTFCKq0=,ssl=True,abortConnect=False";
        public const string ConnectionStrings = @"
sedc-redis-cache.redis.cache.windows.net:6380,
password=lxakE7VAVpdCQSmb0JFwhl7lXg2C07bj68XLyTFCKq0=,
ssl=True,
abortConnect=False";


        static void Main(string[] args)
        {
            var task = Task.Run(() => Get());
            task.Wait();
        }

        static void Get()
        {
            using (var redis = ConnectionMultiplexer.Connect(ConnectionString))
            {
                IDatabase db = redis.GetDatabase();
                var str = db.StringGet("asdasd");
                Console.WriteLine(str);
            }
        }

        static async Task GetAsync()
        {
            using (var redis = ConnectionMultiplexer.Connect(ConnectionString))
            {
                IDatabase db = redis.GetDatabase();
                var str = await db.StringGetAsync("asdasd");
                Console.WriteLine(str);
            }
        }

        static void Set()
        {
            using (var redis = ConnectionMultiplexer.Connect(ConnectionString))
            {
                IDatabase db = redis.GetDatabase();
                var key = "favoritemeal";

                var value = $"{{\"id\":\"{Guid.NewGuid() }\",\"value\"=\"kikiriki\"}}";

                var isSuccessfull = db.StringSet(key, value);

                if (isSuccessfull)
                    Console.WriteLine("success");
                else
                    Console.WriteLine("failed");
            }
        }

        static async Task SetAsync()
        {
            using (var redis = ConnectionMultiplexer.Connect(ConnectionString))
            {
                IDatabase db = redis.GetDatabase();
                var key = "favoritemeal";

                var value = $"{{\"id\":\"{ Guid.NewGuid() }\",\"value\"=\"kikiriki\"}}";

                var sb = new StringBuilder(string.Empty);
                sb.AppendLine("[");

                Enumerable
                    .Range(1, 100)
                    .ToList()
                    .ForEach(x =>
                        sb.AppendLine($"{{\"id\":\"{ Guid.NewGuid() }\",\"value\"=\"kikiriki\"}},")
                    );

                sb.AppendLine($"{{\"id\":\"{ Guid.NewGuid() }\",\"value\"=\"kikiriki\"}}");
                sb.AppendLine("]");

                var bigValue = sb.ToString();
                var sw = new Stopwatch();

                sw.Start();

                var isSuccessfull = await db.StringSetAsync(key, bigValue, null, When.Always, CommandFlags.None);//.GetAwaiter().GetResult();

                sw.Stop();

                Console.WriteLine($"milliseconds: {sw.ElapsedMilliseconds}");
                Console.WriteLine($"time: {sw.Elapsed}");

                if (isSuccessfull)
                    Console.WriteLine("success");
                else
                    Console.WriteLine("failed");

                Console.WriteLine("this should be at the end");
            }
        }
        
    }
}
