using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace Pixel.GLES;

public class Shader
{
    public int Program {get; init;}

    private Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
    private Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

    // This is how you create a simple shader.
    // Shaders are written in GLSL, which is a language very similar to C in its semantics.
    // The GLSL source is compiled *at runtime*, so it can optimize itself for the graphics card it's currently being used on.
    // A commented example of GLSL can be found in shader.vert.

    public Shader() 
    {
        // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
        // To do this, create a program...
        Program = GL.CreateProgram();
    }
    public Shader Load(string vertPath, string fragPath, System.Collections.Generic.IEnumerable<string> attributes)
    {
        // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
        // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
        //   The vertex shader won't be too important here, but they'll be more important later.
        // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
        //   The fragment shader is what we'll be using the most here.

        // Load vertex shader and compile
        var shaderSource = File.ReadAllText(vertPath);

        // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);

        // Now, bind the GLSL source code
        GL.ShaderSource(vertexShader, shaderSource);

        // And then compile
        CompileShader(vertexShader);

        // We do the same for the fragment shader.
        shaderSource = File.ReadAllText(fragPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        CompileShader(fragmentShader);


        // Attach both shaders...
        GL.AttachShader(Program, vertexShader);
        GL.AttachShader(Program, fragmentShader);

        // And then link them together.
        LinkProgram(Program);

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        GL.DetachShader(Program, vertexShader);
        GL.DetachShader(Program, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
        // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
        // later.
        
        GL.GetProgram(Program, GetProgramParameterName.ActiveAttributes, out var numberOfAttributes);
        if(attributes.Count() != numberOfAttributes)
            throw new Exception("[Shader] Attributes not matched");
        foreach (var attrName in attributes)
        {
            attributeLocations.Add(attrName, GL.GetAttribLocation(Program, attrName));
        }

        // First, we have to get the number of active uniforms in the shader.
        GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(Program, i, out _, out _);
            key = System.Text.RegularExpressions.Regex.Replace(key, @"\[\d+\]", string.Empty);
            // get the location,
            var location = GL.GetUniformLocation(Program, key);

            // and then add it to the dictionary.
            uniformLocations.Add(key, location);
        }
        return this;
    }

    private static void CompileShader(int shader)
    {
        // Try to compile the shader
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    private static void LinkProgram(int program)
    {
        // We link the program
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    // A wrapper function that enables the shader program.
    public void Use()
    {
        GL.UseProgram(Program);
        foreach (var kv in this.attributeLocations)
        {
            GL.EnableVertexAttribArray(kv.Value);
        }
    }

    public int this [string name]
    {
        get => this.attributeLocations.ContainsKey(name) ? this.attributeLocations[name] : this.uniformLocations[name];
    }

}
