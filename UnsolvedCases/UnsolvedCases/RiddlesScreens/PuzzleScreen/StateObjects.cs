#region File Description
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class StateObjects
{
    static AlphaTestEffect alphaEffect;

    public static AlphaTestEffect AlphaEffect(GraphicsDevice graphics)
    {
        if (alphaEffect == null)
        {
            alphaEffect = new AlphaTestEffect(graphics);
            alphaEffect.AlphaFunction = CompareFunction.Greater;
            alphaEffect.ReferenceAlpha = 0;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, graphics.PresentationParameters.BackBufferWidth, 
                graphics.PresentationParameters.BackBufferHeight, 0, 0, 1);
            alphaEffect.Projection = projection;
        }
        return alphaEffect;
    }

    public static DepthStencilState StencilMaskBefore = new DepthStencilState()
    {
        StencilEnable = true,
        ReferenceStencil = 1,
        StencilFunction = CompareFunction.Always,
        StencilPass = StencilOperation.Replace,
    };

    public static DepthStencilState StencilMaskAfter = new DepthStencilState()
    {
        StencilEnable = true,
        StencilFunction = CompareFunction.Equal,
        ReferenceStencil = 1,
        StencilPass = StencilOperation.Keep,
    };

}

