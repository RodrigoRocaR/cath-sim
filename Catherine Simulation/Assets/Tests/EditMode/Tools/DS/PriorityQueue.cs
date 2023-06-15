using NUnit.Framework;
using Tools.DS;

namespace Tests.EditMode.Tools.DS
{
    public class PriorityQueue
    {

        [Test]
        public void TestEnqueueDequeue()
        {
            var pq = new PriorityQueue<int>();
            pq.Enqueue(6);
            pq.Enqueue(3);
            pq.Enqueue(10);

            Assert.AreEqual(3, pq.Dequeue());
            Assert.AreEqual(6, pq.Dequeue());
            Assert.AreEqual(10, pq.Dequeue());
        }
        
        [Test]
        public void TestPeek()
        {
            var pq = new PriorityQueue<int>();
            pq.Enqueue(6);
            pq.Enqueue(3);
            pq.Enqueue(10);

            Assert.AreEqual(3, pq.Peek());
            Assert.AreEqual(3, pq.Dequeue());
        }
    }
}