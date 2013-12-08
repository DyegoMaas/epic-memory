using System.Collections.Generic;

public class GerenciadorGUI
{
    private readonly List<IGuiListener> listeners = new List<IGuiListener>();
    
    public void Registrar(IGuiListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void MostrarBotaoComecar()
    {
        listeners.ForEach(l => l.MostrarBotaoComecar());
    }

}