using System.Collections.Generic;
using EpicMemory.Sequenciador;

public class SelecaoPersonagens : InjectionBehaviour
{
    [InjectedDependency] private IInputManager inputManager;

    private readonly Stack<IPersonagem> personagensSelecionados = new Stack<IPersonagem>(2);
    private bool selecaoHabilitada;

    public Queue<Ataque> AtaquesGerados { get; private set; }

    protected override void StartOverride()
    {
        AtaquesGerados = new Queue<Ataque>(2);    
    }

    public void Habilitar()
    {
        selecaoHabilitada = true;
    }

    public void Desabilitar()
    {
        selecaoHabilitada = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!selecaoHabilitada)
            return;

        IPersonagem personagem;
        if (inputManager.Click(out personagem))
        {
            SelecionarPersonagem(personagem);
        }

        if (JogadorCompletouUmAtaque())
        {
            var alvo = personagensSelecionados.Pop();
            var atacante = personagensSelecionados.Pop();
            var ataque = new Ataque(atacante, alvo);

            AtaquesGerados.Enqueue(ataque);
        }
    }

    private void SelecionarPersonagem(IPersonagem personagem)
    {
        if (personagensSelecionados.Count == 1)
        {
            if (OJogadorEscolheuUmPersonagemDoMesmoTime(personagem))
            {
                return;
            }
        }

        personagensSelecionados.Push(personagem);
        personagem.Selecionar();
    }

    private bool OJogadorEscolheuUmPersonagemDoMesmoTime(IPersonagem personagem)
    {
        var personagemJaSelecionado = personagensSelecionados.Pop();
        bool mesmoTime = personagem.Equipe == personagemJaSelecionado.Equipe;
        personagensSelecionados.Push(personagemJaSelecionado);

        return mesmoTime;
    }

    private bool JogadorCompletouUmAtaque()
    {
        return personagensSelecionados.Count == 2;
    }
}
