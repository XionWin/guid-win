namespace NVG;

public static class PerfGraph
{
    public const int GRAPH_HISTORY_COUNT = 100;

    static int style;
    static string name;
    static float[] values;
    static int head;

    public static void InitGraph(int style, string name)
    {
        PerfGraph.style = style;
        PerfGraph.name = name;
        values = new float[GRAPH_HISTORY_COUNT];
        head = 0;
    }

    public static void UpdateGraph(float frameTime)
    {
        head = (head + 1) % GRAPH_HISTORY_COUNT;
        values[head] = frameTime;
    }

    public static float GetGraphAverage()
    {
        int i;
        float avg = 0;
        for (i = 0; i < GRAPH_HISTORY_COUNT; i++)
        {
            avg += values[i];
        }
        return avg / (float)GRAPH_HISTORY_COUNT;
    }

    public static void RenderGraph(NVGcontext vg, float x, float y)
    {
        int i;
        float avg, w, h;
        string str;

        avg = GetGraphAverage();

        w = 200;
        h = 35;

        NanoVG.nvgBeginPath(vg);
        NanoVG.nvgRect(vg, x, y, w, h);
        NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(0, 0, 0, 128));
        NanoVG.nvgFill(vg);

        NanoVG.nvgBeginPath(vg);
        NanoVG.nvgMoveTo(vg, x, y + h);
        if (style == (int)GraphrenderStyle.GRAPH_RENDER_FPS)
        {
            for (i = 0; i < GRAPH_HISTORY_COUNT; i++)
            {
                float v = 1.0f / (0.00001f + values[(head + i) % GRAPH_HISTORY_COUNT]);
                float vx, vy;
                if (v > 80.0f)
                    v = 80.0f;
                vx = x + ((float)i / (GRAPH_HISTORY_COUNT - 1)) * w;
                vy = y + h - ((v / 80.0f) * h);
                NanoVG.nvgLineTo(vg, vx, vy);
            }
        }
        else if (style == (int)GraphrenderStyle.GRAPH_RENDER_PERCENT)
        {
            for (i = 0; i < GRAPH_HISTORY_COUNT; i++)
            {
                float v = values[(head + i) % GRAPH_HISTORY_COUNT] * 1.0f;
                float vx, vy;
                if (v > 100.0f)
                    v = 100.0f;
                vx = x + ((float)i / (GRAPH_HISTORY_COUNT - 1)) * w;
                vy = y + h - ((v / 100.0f) * h);
                NanoVG.nvgLineTo(vg, vx, vy);
            }
        }
        else
        {
            for (i = 0; i < GRAPH_HISTORY_COUNT; i++)
            {
                float v = values[(head + i) % GRAPH_HISTORY_COUNT] * 1000.0f;
                float vx, vy;
                if (v > 20.0f)
                    v = 20.0f;
                vx = x + ((float)i / (GRAPH_HISTORY_COUNT - 1)) * w;
                vy = y + h - ((v / 20.0f) * h);
                NanoVG.nvgLineTo(vg, vx, vy);
            }
        }
        NanoVG.nvgLineTo(vg, x + w, y + h);
        NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(255, 192, 0, 128));
        NanoVG.nvgFill(vg);

        NanoVG.nvgFontFace(vg, "sans");

        if (name[0] != '\0')
        {
            NanoVG.nvgFontSize(vg, 14.0f);
            NanoVG.nvgTextAlign(vg, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(240, 240, 240, 192));
            NanoVG.nvgText(vg, x + 3, y + 1, name);
        }

        if (style == (int)GraphrenderStyle.GRAPH_RENDER_FPS)
        {
            NanoVG.nvgFontSize(vg, 18.0f);
            NanoVG.nvgTextAlign(vg, (int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_TOP));
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(240, 240, 240, 255));
            str = String.Format("{0:0.00} FPS", 1.0f / avg);
            NanoVG.nvgText(vg, x + w - 3, y + 1, str);

            NanoVG.nvgFontSize(vg, 15.0f);
            NanoVG.nvgTextAlign(vg, (int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_BOTTOM));
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(240, 240, 240, 160));
            str = String.Format("{0:0.00} ms", avg * 1000.0f);
            NanoVG.nvgText(vg, x + w - 3, y + h - 1, str);
        }
        else if (style == (int)GraphrenderStyle.GRAPH_RENDER_PERCENT)
        {
            NanoVG.nvgFontSize(vg, 18.0f);
            NanoVG.nvgTextAlign(vg, (int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_TOP));
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(240, 240, 240, 255));
            str = String.Format("{0:0.0} %", avg * 1.0f);
            NanoVG.nvgText(vg, x + w - 3, y + 1, str);
        }
        else
        {
            NanoVG.nvgFontSize(vg, 18.0f);
            NanoVG.nvgTextAlign(vg, (int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_TOP));
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(240, 240, 240, 255));
            str = String.Format("{0:0.00} ms", avg * 1000.0f);
            NanoVG.nvgText(vg, x + w - 3, y + 1, str);
        }
    }
}