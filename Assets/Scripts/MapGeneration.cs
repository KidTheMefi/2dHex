using System;
using Zenject;

public class MapGeneration : IInitializable
{
    public event Action MapGenerated = delegate { };
    private HexMapContinents _hexMapContinents;
    private RiverGenerator _riverGenerator;
    private TestButtonUI.Factory _buttonFactory;
    
    
    public MapGeneration( HexMapContinents hexMapContinents, RiverGenerator riverGenerator, TestButtonUI.Factory buttonFactory)
    {
        _hexMapContinents = hexMapContinents;
        _riverGenerator = riverGenerator;
        _buttonFactory = buttonFactory;
    }



    public void Initialize()
    {
        var buttonUI = _buttonFactory.Create();
        buttonUI.Init(GenerateStart);
    }
    
    private async void GenerateStart()
    {
        await _hexMapContinents.GenerateStart();
        await _riverGenerator.DrawRandomRivers(6);
        
        MapGenerated.Invoke();
    }
    
}
