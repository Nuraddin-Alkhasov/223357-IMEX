using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.Recipe;
using HMI.Reporting;

namespace HMI.Reporting.DataSets.Recipe
{
    partial class RecipeFileInfoDataSet
    {
        partial class RecipeFileInfoDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            /// <summary>
            /// Füllt die DataTable mit den Informationen, die in den Listenelementen enthalten sind.
            /// </summary>
            /// <param name="recipeFiles">Liste mit den IRecipeFile-Objekten, die eingefügt werden sollen.</param>
            public async Task FillWithAsync(IEnumerable<IRecipeFile> recipeFiles, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(recipeFiles, cancellationToken));
            private async Task FillWithAsyncInternal(IEnumerable<IRecipeFile> recipeFiles, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (recipeFiles != null)
                    foreach (var recipeFile in await TaskHelper.RunOnUIThread(() => recipeFiles.ToArray(), cancellationToken))
                    {
                        await this.FillWithAsync(recipeFile, cancellationToken);
                    }
            }

            /// <summary>
            /// Fügt die Informationen des übergeben IRecipeFile-Objekts in die DataTable ein.
            /// </summary>
            /// <param name="recipeFile">Das Objekt, dessen Informationen eingefügt werden sollen.</param>
            public async Task FillWithAsync(IRecipeFile recipeFile, CancellationToken cancellationToken = default(CancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (recipeFile != null)
                {
                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddRecipeFileInfoRow(recipeFile.FileName, recipeFile.Description, recipeFile.TimeOfLastChange, recipeFile.WhoSavedRecipe());
                        }, cancellationToken);
                    }
                    finally
                    {
                        _addLock.Release();
                    }
                }
            }
        }
    }
}