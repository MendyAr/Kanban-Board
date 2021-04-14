using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;


namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Service my_service = new Service();
            Response response;
            response = my_service.Register("mendol@post.bgu.ac.il", "Abcd123");
            Console.WriteLine("ErrorOccured? " + response.ErrorOccured +", " + response.ErrorMessage);

            response = my_service.Register("mendol@post.bgu.ac.il", "Abcd123");
            Console.WriteLine("ErrorOccured? " + response.ErrorOccured + ", " + response.ErrorMessage);

        }
    }
}
