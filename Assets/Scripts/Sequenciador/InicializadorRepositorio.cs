using Assets.Scripts;

public class InicializadorRepositorio : InjectionBehaviour
{
    [InjectedDependency] 
    private RepositorioPersonagens repositorio;

    protected override void StartOverride()
    {
        AdicionarTodosOsPersonagensNoRepositorio();
    }

    private void AdicionarTodosOsPersonagensNoRepositorio()
    {
        var personagens = FindObjectsOfType(typeof(Personagem)) as Personagem[];
        if (personagens != null)
        {
            foreach (var personagem in personagens)
            {
                repositorio.Adicionar(personagem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
