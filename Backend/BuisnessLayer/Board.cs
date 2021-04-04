using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Board
    {
        //fields
        private String id;
        private String name;
        private String creatorEmail;
        private int maxBacklog = -1;
        private int maxInProgress = -1;
        private int maxDone = -1;
        private LinkedList<Task> backlog = new LinkedList<Task>;
        private LinkedList<Task> inProgress= new LinkedList<Task>;
        private LinkedList<Task> done = new LinkedList<Task>;

        //constructor
        public Board(String name, String creatorEmail)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.id = name + creatorEmail;
        }

        //methods
        public void changeNa
    }
}
