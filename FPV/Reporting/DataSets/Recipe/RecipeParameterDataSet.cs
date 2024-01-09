using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;
using VisiWin.Recipe;
using HMI.Reporting;

namespace HMI.Reporting.DataSets.Recipe
{
    partial class RecipeParameterDataSet
    {
        private static readonly IRecipeService recipeService = ApplicationService.GetService<IRecipeService>();
        private static readonly IVariableService variableService = ApplicationService.GetService<IVariableService>();

        partial class RecipeParameterDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            /// <summary>
            /// Befüllt die DataTable mit den Informationen der Parameter einer Rezeptklasse und den
            /// Werten aus dem Rezept-Buffer oder einer Rezeptdatei.
            /// Wenn für den Parameter <paramref name="recipeFileName" /> kein Wert angegeben wird, werden die Werte
            /// aus dem Rezept-Buffer ausgelesen, wenn ein Wert angegeben wird, werden die Werte aus der angegeben
            /// Rezeptdatei ausgelesen.
            /// </summary>
            /// <param name="recipeClassName"></param>
            /// <param name="recipeFileName"></param>
            public async Task FillWithAsync(string recipeClassName, string recipeFileName = null, Func<bool, string> boolConverter = null, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(recipeClassName, recipeFileName, boolConverter, cancellationToken));
            private async Task FillWithAsyncInternal(string recipeClassName, string recipeFileName, Func<bool, string> boolConverter, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!string.IsNullOrEmpty(recipeClassName))
                {
                    var recipeClass = await TaskHelper.RunOnUIThread(() => recipeService.GetRecipeClass(recipeClassName), cancellationToken);

                    if (recipeClass != null)
                    {
                        var valueGetter = string.IsNullOrEmpty(recipeFileName) ?
                            this.GetValueGetterFromBuffer(recipeClass, cancellationToken) :
                            await this.GetValueGetterFromFile(recipeClassName, recipeFileName, cancellationToken);

                        var recipeVars = await TaskHelper.RunOnUIThread(() => recipeClass.GetVariableNames()?.ToList(), cancellationToken);
                        if (recipeVars != null)
                            foreach (var var in recipeVars)
                            {
                                var r = await TaskHelper.RunOnUIThread(() => new
                                {
                                    rv = variableService.GetVariable(var),
                                    rvDef = variableService.GetVariableDefinition(var)
                                }, cancellationToken);
                                var r2 = await TaskHelper.RunOnUIThread(() => new
                                {
                                    name = r.rv.Name,
                                    typeCode = r.rv.TypeCode
                                }, cancellationToken);

                                var value = await valueGetter.Invoke(r2.name);

                                if (r2.typeCode == TypeCode.Boolean)
                                {
                                    value = await TaskHelper.RunOnUIThread(() => boolConverter == null ? value.ToString() : boolConverter.Invoke((bool)value), cancellationToken);
                                }

                                await _addLock.WaitAsync(cancellationToken);
                                try
                                {
                                    await TaskHelper.RunOnUIThread(() =>
                                    {
                                        this.AddRecipeParameterRow(ApplicationService.GetText(r.rvDef.LocalizableText), value != null ? value.ToString() : "", r.rv.Comment,
                                                                r.rv.TypeCode.ToString().Equals("String") ? "" : r.rv.MaxValue.ToString(),
                                                                r.rv.TypeCode.ToString().Equals("String") ? "" : r.rv.MinValue.ToString(), r.rv.Name,
                                                                r.rv.RawTypeCode.ToString(), r.rv.TypeCode.ToString(), r.rv.UnitText);
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

            /// <summary>
            /// Diese Methode erzeugt eine Funktion, die Werte aus dem Rezept-Buffer ausliest.
            /// </summary>
            /// <param name="recipeClass"></param>
            /// <returns></returns>
            private Func<string, Task<object>> GetValueGetterFromBuffer(IRecipeClass recipeClass, CancellationToken cancellationToken = default(CancellationToken))
            {
                return async s =>
                    {
                        object value = null;
                        if (await TaskHelper.RunOnUIThread(() => !recipeClass.GetValue(s, out value), cancellationToken))
                        {
                            value = "Cannot determine value.";
                        }
                        return value;
                    };
            }

            /// <summary>
            /// Diese Methode erzeugt eine Funktion die Werte aus einer bestimmten Rezept-Datei ausliest.
            /// </summary>
            /// <param name="recipeClassName"></param>
            /// <param name="fileName"></param>
            /// <returns>Eine Funktion die den Wert ein</returns>
            private async Task<Func<string, Task<object>>> GetValueGetterFromFile(string recipeClassName, string fileName, CancellationToken cancellationToken = default(CancellationToken))
            {
                var values = await TaskHelper.RunOnUIThread(() => recipeService.LoadRecipeFileValues(recipeClassName, fileName), cancellationToken);
                return async s => { return await TaskHelper.RunOnUIThread(() => values[s], cancellationToken); };
            }
        }

        partial class RecipeParameterWithChangesDataTable
        {
            private readonly SemaphoreSlim _addLock = new SemaphoreSlim(1, 1);

            public async Task FillWithAsync(string recipeClassName, string recipeFileName = null, Func<bool, string> boolConverter = null, CancellationToken cancellationToken = default(CancellationToken)) => await Task.Run(() => FillWithAsyncInternal(recipeClassName, recipeFileName, boolConverter, cancellationToken));
            private async Task FillWithAsyncInternal(string recipeClassName, string recipeFileName, Func<bool, string> boolConverter, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var parameterDataTable = new RecipeParameterDataTable();
                await parameterDataTable.FillWithAsync(recipeClassName, recipeFileName, boolConverter, cancellationToken);

                var changes = await TaskHelper.RunOnUIThread(() => recipeService.GetRecipeClass(recipeClassName)?.GetChanges()?.ToList(), cancellationToken);
                if (changes != null)
                    foreach (var parameter in parameterDataTable)
                    {
                        if (changes != null && changes.Count > 0)
                        {
                            var aChanges = await TaskHelper.RunOnUIThread(() => changes.Where(item => item.Variable == parameter.Name), cancellationToken);
                            await this.FillWithAsyncInternal(parameter, aChanges, boolConverter, cancellationToken);
                        }
                        else
                        {
                            await this.FillWithAsyncInternal(parameter, null as IEnumerable<IRecipeChange>, null, cancellationToken);
                        }
                    }
            }

            private async Task FillWithAsyncInternal(RecipeParameterRow parameter, IEnumerable<IRecipeChange> aChanges, Func<bool, string> boolConverter, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var aChangesList = await TaskHelper.RunOnUIThread(() => aChanges?.ToList(), cancellationToken);
                if (aChangesList != null && aChangesList.Count > 0)
                {
                    foreach (var changeItem in aChangesList)
                    {
                        await this.FillWithAsync(parameter, changeItem, boolConverter, cancellationToken);
                    }
                }
                else
                {
                    await this.FillWithAsync(parameter, null as IRecipeChange, null, cancellationToken);
                }
            }

            private async Task FillWithAsync(RecipeParameterRow parameter, IRecipeChange changeItem, Func<bool, string> boolConverter, CancellationToken cancellationToken)
            {
                if (parameter != null)
                {
                    string newValue = "", oldValue = "";
                    if (changeItem != null)
                    {
                        var r = await TaskHelper.RunOnUIThread(() => new { changeItem.NewValue, changeItem.OldValue }, cancellationToken);

                        if (parameter.Type == "Boolean" && boolConverter != null)
                        {
                            newValue = boolConverter.Invoke((bool)r.NewValue);
                            oldValue = boolConverter.Invoke((bool)r.OldValue);
                        }
                        else
                        {
                            newValue = r.NewValue?.ToString();
                            oldValue = r.OldValue?.ToString();
                        }
                    }

                    await _addLock.WaitAsync(cancellationToken);
                    try
                    {
                        await TaskHelper.RunOnUIThread(() =>
                        {
                            this.AddRecipeParameterWithChangesRow(parameter.LocalizedParameterName, parameter.Value, parameter.Description, parameter.Maximum, parameter.Minimum, parameter.Name,
                                        parameter.RawType, parameter.Type, parameter.Unit, newValue, oldValue, changeItem?.Tag?.ToString(), changeItem == null ? default(DateTime) : changeItem.TimeStamp,
                                        changeItem?.UserName, changeItem == null ? parameter.Name : changeItem.Variable, changeItem != null);
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