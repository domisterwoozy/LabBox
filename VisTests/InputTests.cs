using FakeItEasy;
using LabBox.Visualization.Input;
using LabBox.Visualization.Universe.Interaction;
using Math3D;
using NUnit.Framework;
using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace VisTests
{    
    public class InputTests
    {
        private static IInputObservable CreateFakeInput(IInput[] inputEvents)
        {
            var input = A.Fake<IInputObservable>();
            var inputObs = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(inputEvents.Length).Select(i => inputEvents[i]); // spaces out the input by 1/10 of a second
            A.CallTo(() => input.InputEvents).Returns(inputObs);
            return input;
        }

        // repeatedly select one item
        [Test]
        public void SelectorTestOne()
        {            
            var item = new object();          
            var inputEvts = new IInput[]
            {
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0)
            };

            var input = CreateFakeInput(inputEvts);
            var selector = new ItemSelector<object>(input, () => item.ToOptional());

            input.InputEvents.Wait();
            Assert.That(selector.SelectedItems.Count, Is.EqualTo(1));
            Assert.That(selector.SelectedItems.First(), Is.EqualTo(item));
        }

        // repeatedly select new objects with multi select down
        [Test]
        public void SelectorTestTwo()
        {           
            var inputEvts = new IInput[]
            {
                new BasicInput(InputType.MultiSelect, InputState.Start, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.MultiSelect, InputState.Finish, 1.0),
            };
            var input = CreateFakeInput(inputEvts);
            var selector = new ItemSelector<object>(input, () => new object().ToOptional());

            input.InputEvents.Wait();
            Assert.That(selector.SelectedItems.Count, Is.EqualTo(4));
        }

        // repeatedly select new objects with no multi select
        [Test]
        public void SelectorTestThree()
        {
            var inputEvts = new IInput[]
            {
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
                new BasicInput(InputType.PrimarySelect, InputState.Finish, 1.0),
            };
            var input = CreateFakeInput(inputEvts);
            var selector = new ItemSelector<object>(input, () => new object().ToOptional());

            input.InputEvents.Wait();
            Assert.That(selector.SelectedItems.Count, Is.EqualTo(1));
        }
    }
}
