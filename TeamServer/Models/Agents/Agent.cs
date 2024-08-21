using System.Collections.Concurrent;
namespace TeamServer.Models.Agents
{
    public class Agent
    {
        public AgentMetadata Metadata {get;  }

        public DateTime LastSeen { get; private set; }


        private readonly ConcurrentQueue<AgentTask> _pendingTasks = new();
        public Agent(AgentMetadata metadata)
        {

            Metadata = metadata;
        }
        public void CheckIn()
        {

            LastSeen = DateTime.UtcNow;
            
        }

        public void QueueTak(AgentTask task)
        {

            _pendingTasks.Enqueue(task);
        }
        public IEnumerable <AgentTask> GetPendingTask() {


            List<AgentTask> tasks = new();
            while (_pendingTasks.TryDequeue(out var task)) 
            { 
             tasks.Add(task);
            
            }
                return tasks;

        }


    }
}
