using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccordTest {
    class Program {
        static void Main(string[] args) {
            var network = new DeepBeliefNetwork(new BernoulliFunction(), 1024, 50, 10);
            var teacher = new BackPropagationLearning(Main.Network) {
                LearningRate = 0.1,
                Momentum = 0.9
            };
        }
    }
}
