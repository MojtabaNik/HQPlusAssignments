using System.Collections.Generic;

namespace HQPlusAssignments.Application.Test.HtmlExtractor
{
    public class GetValuesFromNodesOutPut
    {
        public string Head { get; set; }
        public string H1Text { get; set; }
        public float H2Text { get; set; }
        public List<Car> Cars { get; set; }
    }

    public class CarModel
    {
        public string Name { get; set; }
    }

    public class Car
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public List<CarModel> CarModels { get; set; }
    }
}
