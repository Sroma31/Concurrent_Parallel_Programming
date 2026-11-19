using System.Dynamic;

public class ProducerConsumerExperiment
{
    private const int BufferSize = 4; //grandezza della tavole in cui metter i piatti con il cibo
    private const int Nproducer = 2; 
    private const int Nconsumers = 2;

    public void Run()
    {
        var producersSemaphore = new SemaphoreSlim(BufferSize, BufferSize); //semaforo per i produttori
    }

}