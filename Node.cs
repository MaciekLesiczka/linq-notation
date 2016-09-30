namespace LinqNotation
{
    /*
        class NodeSample
        {
            public class Node
            {
                public Node Parent { get; set; }

                public Node GranGrandPa =>
                    from parent in Parent
                    from grandPa in parent.Parent
                    select grandPa.Parent;

                private Tuple<Node, int> Sample()
                {
                    return from parent in Parent
                           from grandPa in parent.Parent
                           let now = DateTime.Now
                           from details in GetNodeDetails(parent, now)
                           select Tuple.Create(grandPa.Parent, details);
                }



                public Node GranGrandPa2 => Parent?.Parent?.Parent;


                public int GetNodeDetails(Node parent, DateTime now)
                {
                    return 0;
                }
            }

            static class NodeExt
            {
                public static TResult SelectMany<TSource, TTemp, TResult>(this TSource _this, Func<TSource, TTemp> optionSelector, Func<TSource, TTemp, TResult> resultSelector)
                    where TResult : class
                {
                    if (_this != null)
                    {
                        var temp = optionSelector(_this);
                        if (temp != null)
                        {
                            return resultSelector(_this, temp);
                        }
                    }
                    return null;
                }

                public static TResult Select<TSource, TResult>(this TSource _this, Func<TSource, TResult> resultSelector)
                    where TResult : class
                {
                    if (_this != null)
                    {
                        return resultSelector(_this);


                    }
                    return null;
                }
            }
        }
        */

}
