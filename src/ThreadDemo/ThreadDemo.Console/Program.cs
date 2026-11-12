using ThreadDemo.Domain;

var experiments = new ThreadExperiments();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("THREAD DEMO");
    Console.WriteLine("1. Esecuzione sequenziale");
    Console.WriteLine("2. Due thread concorrenti");
    Console.WriteLine("3. Due thread in background");
    Console.WriteLine("4. Proprietà del thread corrente");
    Console.WriteLine("5. Contatore senza lock");
    Console.WriteLine("6. Contatore con lock");
    Console.WriteLine("0. Esci");
    Console.Write("Scelta: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            experiments.RunSequential();
            break;

        case "2":
            experiments.RunThreads();
            break;

        case "3":
            experiments.RunBackgroundThread();
            break;

        case "4":
            experiments.ShowThreadProperties();
            break;

        case "5":
        case "6":
            var result = experiments.RunCounter(useLock: choice == "5");

            Console.WriteLine($"Atteso: {result.Expected}");
            Console.WriteLine($"Ottenuto: {result.Actual}");
            break;


        case "0":
            return;

        default:
            Console.WriteLine("Scelta non valida.");
            break;
    }

    Console.WriteLine("Premi INVIO per continuare...");
    Console.ReadLine();
}