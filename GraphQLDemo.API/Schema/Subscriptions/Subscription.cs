using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Subscriptions
{
    public class Subscription
    {
        /*
        subscription
        {
            courseCreated {
            id
            name
            subject
            }
        }
         */
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

        /*
         * subscription{
              courseUpdated(courseId: "911dfc8d-1b86-428d-897d-eeb416bf1359") {
                id
                name
              }
            }
         */
        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topic = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
            return topicEventReceiver.SubscribeAsync<string, CourseResult>(topic);
        }
    }
}
