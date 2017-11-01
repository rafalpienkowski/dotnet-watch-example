using System;
using Xunit;

namespace ExampleMvcApp.Tests
{
    public class SimpleTest
    {
        [Fact]
        public void SimpleTest_WithoutConditions_ShouldPass()
        {
            // I'm aware that this test is pointless. 
            // Its purpose is only to show dotnet watch test possibilities.
            Assert.True(true);
        
        }
    }
}
