using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;

namespace ThreadDemo.Domain;

public sealed class ThreadExperiments
{
    private static int counter = 0;
    private static object gate = new();


    public void RunSequential()
    {
        Work("A");
        Work("B");
    }
    public void RunThreads()
    {
        // lambda function to assign a certain work to a thread in his constructor
        Thread thread1= new(() => Work("A"))
        {
            Name = "Worker-A",
        };


        Thread thread2= new(() => Work("B"))
        {
            Name = "Worker-B",
        };
        Console.WriteLine("Starting threads...");
        
        //this is a non deterministic run
        thread1.Start();
        Thread.Sleep(250);
        thread2.Start();
        Thread.Sleep(250);

        Console.WriteLine("this line is written by main thread");


        //this is a deterministic run
        thread1.Join();//waiting for thread to finish
        thread2.Join();
    }
    public void RunBackgroundThread()
    {
        Thread thread_1 = new Thread(() => Work("A_Background"))
        {
            Name = "Worker-A-Background",
        };      

        Thread thread_2 = new Thread(() => Work("B_Background"))
        {
            Name = "Worker-B-Background",
        };

        thread_1.IsBackground = true;
        thread_1.Start();
        Thread.Sleep(250);
        thread_2.Start();
        Thread.Sleep(250);


        Thread.Sleep(500);

        thread_1.Join();
        thread_2.Join();
    }
    public void ShowThreadProperties()
    {

        Thread thread = Thread.CurrentThread;
        Console.WriteLine($"Thread ID: {thread.ManagedThreadId}");
        Console.WriteLine($"Thread Name: {thread.Name}");
        Console.WriteLine($"Thread Priority: {thread.Priority}");
        Console.WriteLine($"Is Background Thread: {thread. IsBackground}");
        Console.WriteLine($"Is Thread Pool Thread: {thread.IsThreadPoolThread}");
    }
//avere una risorsa condivisa su cui più thread può essere 
//pericoloso in quanto potrebbero esserci problemi 
// sull'aggiornamento della variabile condivisa

//1°test: i thread contano sulla stessa varabile condivisa senza sincronizzazione
//2° test: i thread contano sulla stessa varabile condivisa con sincronizzazione
//il metodo ritorna 2 valori(una tupla) -> (Expected, Actual)
    public (int Expected, int Actual) RunCounter(bool useLock, int iterations=100_000)
    {
        counter = 0;
        
        Thread t1 = new Thread(() => Increment(iterations, useLock));
        Thread t2 = new Thread(() => Increment(iterations, useLock));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        return (iterations*2, counter);
    }
    //function used by our thread 
    public static void Increment(int iterations,  bool useLock)
        {
            for(int i=0; i< iterations; i++)
            {
                //mi serve un accesso in mutua esclusione per ottenere una soluzione deterministica
                if (useLock)
                {
                    //sezione critica
                    lock (gate)
                    {
                        counter++;
                    }
                }
                else
                {
                    counter++;
                }
                
                
            }
        }
    //funzione per simulare del lavoro
    private static void Work(string name)
    {
        for (int i=0; i<=5; i++)
        {
            Console.WriteLine($"{name}: Thread: {Thread.CurrentThread.ManagedThreadId}; Index {i}");
            Thread.Sleep(100);
        }
    }

}
