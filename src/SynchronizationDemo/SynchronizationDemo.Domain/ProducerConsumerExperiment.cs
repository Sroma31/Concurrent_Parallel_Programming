using System.Collections;
using System.Dynamic;

public class ProducerConsumerExperiment
{
    private const int BufferSize = 4; //grandezza della tavole in cui metter i piatti con il cibo
    private const int Nproducer = 2; 
    private const int Nconsumers = 2;


    //struttura dati per posizionare i prodotti creati e consumati
    Queue<int> Buffer = new();

    public void Run()
    {
        
        var spaziDisponibili = new SemaphoreSlim(BufferSize, BufferSize); //semaforo per i produttori
        var elementiDisponibili = new SemaphoreSlim(0, BufferSize);
        var accessoTavola = new SemaphoreSlim(1,1);//accesso alla tavola in maniera mutualmente esclusiva


        Thread[] produttori = new Thread[Nproducer];
        Thread[] consumatori = new Thread[Nconsumers];


        int nextId =0;
        //TODO implementazione dei metodi per produrre e per consumare

        void Consumesomeproduct(int consumerId)
        {
            
        }

        void Makesomeproduct(int producerid)
        {
            //per sempre finche il thred non viene bloccato esternamente per altri motivi
            while (true)
            {
                
                //con ref passo l'area di memoria e non una copia relativa a nextId
                int foodItem = Interlocked.Increment(ref nextId);


                // per evitare che più thread creino item identici devo gaantire l'unicità
                spaziDisponibili.Wait();

                try
                {
                    try
                    {
                        accessoTavola.Wait();//se ho questo permesso ovvvero posso accedere alla tavola;                    Buffer
                        Buffer.Enqueue(foodItem); //aggiungo un elemento alla mia coda di produzione
                        //Interlocked.Increment(ref nextId);
                        Console.WriteLine($"producer id: { producerid}, Item{foodItem}, Buffer count: {Buffer.Count}");
                    }
                    finally
                    {  
                        
                        accessoTavola.Release();
                    }

                }
                catch
                {
                    //se fallisce posso avvertire agli altri che possono produrre, restituisco lo spazio
                    spaziDisponibili.Release();
                }


                //segnalare che non è possibile poter accedere a nuovi fooditems ai consumaotori.
                elementiDisponibili.Release();
                Thread.Sleep(Random.Shared.Next(100,250));//i producer aspetteranno empi diversi
            }
        }

        for (int i =1; i< Nconsumers; i++)
        {   

            //i corrisponde all id del thread corrente
            consumatori[i] = new Thread(()=> Consumesomeproduct(i));
            consumatori[i].Start();
        }

        for (int i =1; i< Nproducer; i++)
        {   

            //i corrisponde all id del thread corrente
            produttori[i] = new Thread(()=> Makesomeproduct(i));
            produttori[i].Start();
        }



        //attendo il termin dei cosumatori e dei produttori
        foreach(Thread consumatore in consumatori)
        {
            consumatore.Join();
        }

        foreach(Thread produttore in consumatori)
        {
            produttore.Join();
        }



    }
}