# RGen

![dots-64](assets/dots-64.png) A flexible random value generator for the command-line.

## Examples

### Generate Integers

The following command will generate a single random integer.

```powershell
.\rgen int
```

To generate e.g. three random integers:

```powershell
.\rgen int 3
```

To generate two sets with three random integers in each set:

```powershell
.\rgen int 3 --set 2
```

### Response Files

To facilitate development and experimentation, response files are included in the `rsp` folder for some common input arguments.

To run in PowerShell:

```powershell
.\src\RGen\bin\Debug\net7.0\rgen.exe "@rsp\int.rsp"
```

To run in Windows Terminal:

```bash
src\RGen\bin\Debug\net7.0\rgen.exe @rsp\int.rsp
```

## Attributions

Project icon "Dots" created by [Royyan Wijaya - Flaticon](https://www.flaticon.com/free-icons/dots).