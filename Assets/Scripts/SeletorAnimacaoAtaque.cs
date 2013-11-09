public class SeletorAnimacaoAtaque
{
    private readonly tk2dSpriteAnimator spriteAnimator;

    public SeletorAnimacaoAtaque(tk2dSpriteAnimator spriteAnimator)
    {
        this.spriteAnimator = spriteAnimator;
    }

    public tk2dSpriteAnimationClip BuscarClipe(int nivel)
    {
        string nomeClip = string.Format("Ataque{0:D2}", nivel);
        var clipe = spriteAnimator.GetClipByName(nomeClip);
        if (clipe == null)
            return BuscarClipe(nivel - 1);

        return clipe;
    }
}