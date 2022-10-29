using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// .Net needs to be 3.5

class Program {
    static void Main(string[] args){
        TestingClass x = new TestingClass();

        Console.WriteLine("Started!");

        x.DoSomething();
        MyPatcher.DoPatching();
        x.DoSomething();
    }
}
