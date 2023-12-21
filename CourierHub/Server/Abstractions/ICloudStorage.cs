namespace CourierHub.Shared.Abstractions;

/// <summary>
/// Represents an interface for cloud storage operations.
/// </summary>
public interface ICloudStorage {
    /// <summary>
    /// Checks if a blob exists in the specified container.
    /// </summary>
    /// <param name="path">The path to the blob.</param>
    /// <param name="container">The name of the container.</param>
    /// <returns>
    /// <para>
    /// An asynchronous <see cref="Task{TResult}"/> representing the operation.
    /// </para>
    /// <para>
    /// The result is equal <see langword="true"/> if the blob exists; otherwise, <see langword="false"/>.
    /// </para>
    /// <para>
    /// <typeparamref name="TResult"/> is <see cref="bool"/>
    /// </para>
    /// </returns>
    Task<bool> CheckBlobAsync(string path, string container);

    /// <summary>
    /// Gets the content of a blob from the specified container.
    /// </summary>
    /// <param name="path">The path to the blob.</param>
    /// <param name="container">The name of the container.</param>
    /// <returns>
    /// <para>
    /// An asynchronous <see cref="Task{TResult}"/> representing the operation.
    /// </para>
    /// <para>
    /// The result contains the content of the blob as a <see langword="string"/>.
    /// </para>
    /// <para>
    /// <typeparamref name="TResult"/> is <see langword="string"/>
    /// </para>
    /// </returns>
    Task<string> GetBlobAsync(string path, string container);

    /// <summary>
    /// Uploads a blob to the specified container.
    /// </summary>
    /// <param name="path">The path to the blob.</param>
    /// <param name="container">The name of the container.</param>
    /// <param name="blob">The content of the blob as a string.</param>
    /// <param name="gzip">A flag specifying whether to gzip the file.</param> 
    /// <returns>
    /// An asynchronous <see cref="Task"/> representing the operation.
    /// </returns>
    Task PutBlobAsync(string path, string container, string blob, bool gzip);
}
