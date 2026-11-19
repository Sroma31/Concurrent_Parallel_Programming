namespace SynchronizationDemo.Domain;


//a semaphore is a synchronization primitive that can be used to limit the number of 
// threads that can access a resource concurrently


public class SemaphoreExperiment
{

    public void Run()
    {
        //Resource shared between threads
        //In this case, the resource is a semaphore that allows only one thread to access it at a time
        // 1° parameter: initialCount: the number of threads that can enter the semaphore at the same time
        // 2° parameter: maxCount: the maximum number of threads that can enter the semaphore at the same time
        var resource_1= new SemaphoreSlim(2,2);//semaphoreslim is a lightweight semaphore that can be used to limit the number of threads that can access a resource concurrently
        
        // Workers that will try to access the resource
        Thread[] workers= new Thread[5];

        //access to shared resource:
        for (int i=0; i<workers.Length; i++)
        {
            int idWorker= i+1; // Capture the current value of i for the thread
            workers[i]= new Thread(() =>//instead of lambda expression, we can use a method that takes the idWorker as parameter
            {
                Console.WriteLine($"Worker {idWorker} is waiting to access resource_1");
                resource_1.Wait(); // Wait for the semaphore to be available
                Console.WriteLine($"Worker {idWorker} has entered resource_1");
                Thread.Sleep(1000); // Simulate work
                Console.WriteLine($"Worker {idWorker} is leaving resource_1");
                resource_1.Release(); // Release the semaphore
            });
        }
            
            


        // Create and start the workers
        foreach(Thread worker in workers)
        {
            worker.Start();
        }

        // Wait for all workers to finish
        foreach (Thread worker in workers)
        {
            worker.Join();
        }


        // when all workers have finished, we can release the resource
        Console.WriteLine("End, every worker has finished accessing resource_1"); 
        
    }

}
