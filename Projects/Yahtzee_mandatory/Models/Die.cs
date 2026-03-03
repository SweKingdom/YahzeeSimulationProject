namespace Playground.Projects.Yahtzee.Models;


public enum DiePip {One = 1, Two, Three, Four, Five, Six }   
public record Die
{
    public DiePip Pip { get; init; }
    
    public Die(DiePip pip)
    {
        if (pip < DiePip.One || pip > DiePip.Six)
            throw new ArgumentException("Die value must be between 1 and 6");
        Pip = pip;
    }    
    public override string ToString() => Pip.ToString();
}