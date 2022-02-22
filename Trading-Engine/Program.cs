using Trading_Engine.Application;
using Trading_Engine.Application.Commands;
using Trading_Engine.Domain;

args = new[]
{
    "SUB LO B Ffuj 200 13", "SUB LO B Yy7P 150 11", "SUB LO B YuFU 100 13", "SUB LO S IpD8 150 14",
    "SUB LO S y93N 190 15", "SUB LO B Y5wb 230 14", "SUB MO B IZLO 250", "CXL Ffuj", "CXL 49Ze", "END",
};

var orderBook = new OrderBook();
var actionParser = new ActionReader();
var commandParser = new CommandParser();
var commandHandlers = new List<ICommandHandler>()
{
    new CancelOrderActionCommandHandler(orderBook),
    new CancelOrReplaceActionCommandHandler(orderBook),

    new MarkedOrderCommandHandler(orderBook),
    new LimitOrderCommandHandler(orderBook),
    new ImmediateOrCancelOrderCommandHandler(orderBook),
    new FillOrKillOrderCommandHandler(orderBook),
    new IceBergOrderCommandHandler(orderBook)
};

var matchingEngine = new MatchingEngine(commandParser, commandHandlers);

//var parsedActions = actionParser.LoadActions();
var parsedActions = args.ToList();
var result = matchingEngine.ExecuteActions(parsedActions);
ResultParser.PrintResult(result, orderBook);


class MatchingEngine
{
    private readonly CommandParser _commandParser;
    private readonly List<ICommandHandler> _commandHandlers;

    public MatchingEngine(CommandParser commandParser, List<ICommandHandler> commandHandlers)
    {
        _commandParser = commandParser;
        _commandHandlers = commandHandlers;
    }
    public List<string> ExecuteActions(List<string> actions)
    {
        var executionResult = new List<string>();
        foreach (string action in actions)
        {
            // match with command
            var command = _commandParser.ParseCommand(action);

            if (command == null) continue;

            string result = HandleCommand(command);
            executionResult.Add(result);
        }

        return executionResult;
    }

    // it should be done better, plus it would work normally with some IOC container, i dont want to waste more time for better handling it, but there should not be  any used concrete typ
    public string HandleCommand<T>(T command) where T : ICommand
    {
        if (command is CancelOrderActionCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<CancelOrderActionCommand>>().FirstOrDefault().Handle(command as CancelOrderActionCommand);
        }
        else if (command is CancelOrReplaceActionCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<CancelOrReplaceActionCommand>>().FirstOrDefault().Handle(command as CancelOrReplaceActionCommand);
        }
        else if (command is MarketOrderCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<MarketOrderCommand>>().FirstOrDefault().Handle(command as MarketOrderCommand);
        }
        else if (command is LimitOrderCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<LimitOrderCommand>>().FirstOrDefault().Handle(command as LimitOrderCommand);
        }
        else if (command is ImmediateOrCancelOrderCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<ImmediateOrCancelOrderCommand>>().FirstOrDefault().Handle(command as ImmediateOrCancelOrderCommand);
        }
        else if (command is FillOrKillOrderCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<FillOrKillOrderCommand>>().FirstOrDefault().Handle(command as FillOrKillOrderCommand);
        }
        else if (command is IceBergOrderCommand)
        {
            return _commandHandlers.OfType<ICommandHandler<IceBergOrderCommand>>().FirstOrDefault().Handle(command as IceBergOrderCommand);
        }

        return null;
    }
}

static class ResultParser
{
    public static void PrintResult(List<string> actionsResults, OrderBook orderBook)
    {
        foreach (var result in actionsResults.Where(result => !string.IsNullOrWhiteSpace(result)))
        {
            Console.WriteLine(result);
        }
        Console.WriteLine($"B: {ConvertAllOrders(orderBook.BuyOrders.ToList())}");
        Console.WriteLine($"S: {ConvertAllOrders(orderBook.SellOrders.ToList())}");
    }

    private static string ConvertAllOrders(List<Order> orders) => string.Join(" ", orders.Select(order => order.ToString()));
}

class CommandParser
{
    // i know that this parsing is fragile, it should have some validation
    public ICommand ParseCommand(string rawAction)
    {
        ICommand command = null;
        var values = rawAction.Split(' ');

        if (values[0] == AppConstants.SUBMITTED_ORDER)
        {
            if (values[1] == AppConstants.LIMIT_ORDER)
            {
                command = CreateLimitOrderCommand(values);
            }
            else if (values[1] == AppConstants.MARKET_ORDER)
            {
                command = CreateMarketOrderCommand(values);
            }
            else if (values[1] == AppConstants.IOC_ORDER)
            {
                command = CreateImmediateOrCancelOrderCommand(values);
            }
            else if (values[1] == AppConstants.FOK_ORDER)
            {
                command = CreateFillOrKillOrderCommand(values);
            }
            else if (values[1] == AppConstants.Iceberg_ORDER)
            {
                command = CreateIceBergOrderCommand(values);
            }
        }
        else if (values[0] == AppConstants.CANCEL_ACTION)
        {
            command = CreateCancelOrderCommand(values);
        }

        else if (values[0] == AppConstants.CRP_ACTION)
        {
            command = CancelOrReplaceActionCommandCommand(values);
        }

        return command;
    }

    private CancelOrderActionCommand CreateCancelOrderCommand(string[] values) => new CancelOrderActionCommand(values[1]);
    private CancelOrReplaceActionCommand CancelOrReplaceActionCommandCommand(string[] values) => new CancelOrReplaceActionCommand(
        values[1],
        int.Parse(values[3]),
        int.Parse(values[2])
        );

    private LimitOrderCommand CreateLimitOrderCommand(string[] values) => new LimitOrderCommand(
        values[3],
        int.Parse(values[5]),
        values[2] == AppConstants.ORDER_BUY_SIDE ? SideType.Buy : SideType.Sell,
        int.Parse(values[4]));
    private MarketOrderCommand CreateMarketOrderCommand(string[] values) => new MarketOrderCommand(
        values[3],
        values[2] == AppConstants.ORDER_BUY_SIDE ? SideType.Buy : SideType.Sell,
        int.Parse(values[4]));

    private ImmediateOrCancelOrderCommand CreateImmediateOrCancelOrderCommand(string[] values) => new ImmediateOrCancelOrderCommand(
        values[3],
        int.Parse(values[5]),
        values[2] == AppConstants.ORDER_BUY_SIDE ? SideType.Buy : SideType.Sell,
        int.Parse(values[4]));

    private FillOrKillOrderCommand CreateFillOrKillOrderCommand(string[] values) => new FillOrKillOrderCommand(
        values[3],
        int.Parse(values[5]),
        values[2] == AppConstants.ORDER_BUY_SIDE ? SideType.Buy : SideType.Sell,
        int.Parse(values[4]));

    private IceBergOrderCommand CreateIceBergOrderCommand(string[] values) => new IceBergOrderCommand(
        values[3],
        int.Parse(values[5]),
        values[2] == AppConstants.ORDER_BUY_SIDE ? SideType.Buy : SideType.Sell,
        int.Parse(values[4]),
        int.Parse(values[6]));
}
//END COMMANDS

public class ActionReader
{
    public List<string> LoadActions()
    {
        var listOfActions = new List<string>();

        var currentLine = "";
        while (currentLine != AppConstants.END_INPUT)
        {
            currentLine = Console.ReadLine();
            listOfActions.Add(currentLine);
        }

        return listOfActions;
    }
}
