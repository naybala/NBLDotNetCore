using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBLDotNetCore.ConsoleApp;

internal class EFCoreExample
{
    private readonly AppDbContext db = new AppDbContext();

    public void Run()
    {
        Read();
        Edit(7);
    }

    private void Read()
    {
        var lst = db.Blogs.ToList();
        foreach (BlogDto item in lst)
        {
            Console.WriteLine(item.BlogId);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
            Console.WriteLine("=============================================");
        }
    }

    public void Edit(int id)
    {
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            Console.WriteLine("No Data Found");
            return;
        }
        Console.WriteLine(item.BlogId);
        Console.WriteLine(item.BlogTitle);
        Console.WriteLine(item.BlogAuthor);
        Console.WriteLine(item.BlogContent);
        Console.WriteLine("=============================================");
    }
}
