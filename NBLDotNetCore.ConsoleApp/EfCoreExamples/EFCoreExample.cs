using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBLDotNetCore.ConsoleApp.Dtos;

namespace NBLDotNetCore.ConsoleApp.EfCoreExamples;

internal class EFCoreExample
{
    private readonly AppDbContext db = new AppDbContext();

    public void Run()
    {
        Read();
        Edit(7);
        Create("U Mya Book", "U Mya", "This is Content");
        Update(1, "U Mya Book", "U Mya", "This is Content");
        Delete(1);
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

    private void Create(string title, string author, string content)
    {
        var item = new BlogDto
        {
            BlogTitle = title,
            BlogAuthor = author,
            BlogContent = content
        };
        db.Blogs.Add(item);
        int result = db.SaveChanges();

        string message = result > 0 ? "Inserting Successful" : "Inserting Failed";
        Console.Write(message);
    }

    private void Update(int id, string title, string author, string content)
    {
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            Console.WriteLine("No Data Found");
            return;
        }
        item.BlogTitle = title;
        item.BlogAuthor = author;
        item.BlogContent = content;

        int result = db.SaveChanges();

        string message = result > 0 ? "Updating Successful" : "Updating Failed";
        Console.Write(message);
    }

    private void Delete(int id)
    {
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            Console.WriteLine("No Data Found");
            return;
        }
        db.Blogs.Remove(item);
    }
}
