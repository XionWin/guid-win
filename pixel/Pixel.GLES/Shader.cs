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
    }

    // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
    // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Program, attribName);
    }

    // Uniform setters
    // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
    // You use VBOs for vertex-related data, and uniforms for almost everything else.

    // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
    //     1. Bind the program you want to set the uniform on
    //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
    //     3. Use the appropriate GL.Uniform* function to set the uniform.

    /// <summary>
    /// Set a uniform int on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetInt(string name, int data)
    {
        GL.UseProgram(Program);
        GL.Uniform1(uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform float on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetFloat(string name, float data)
    {
        GL.UseProgram(Program);
        GL.Uniform1(uniformLocations[name], data);
    }

    /// <summary>
    /// Set a uniform Matrix4 on this shader
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    /// <remarks>
    ///   <para>
    ///   The matrix is transposed before being sent to the shader.
    ///   </para>
    /// </remarks>
    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(Program);
        GL.UniformMatrix4(uniformLocations[name], true, ref data);
    }

    /// <summary>
    /// Set a uniform Vector3 on this shader.
    /// </summary>
    /// <param name="name">The name of the uniform</param>
    /// <param name="data">The data to set</param>
    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(Program);
        GL.Uniform3(uniformLocations[name], data);
    }

    public void SetVector2(string name, Vector2 data)
    {
        GL.UseProgram(Program);
        GL.Uniform2(uniformLocations[name], data);
    }
    public void SetVector2(string name, float v0, float v1)
    {
        GL.UseProgram(Program);
        GL.Uniform2(uniformLocations[name], v0, v1);
    }

    
    public void SetUniform4(string name, int size, float[] data)
    {
        GL.UseProgram(Program);
        GL.Uniform4(uniformLocations[name], size, data);
    }
}
