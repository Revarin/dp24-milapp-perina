using Kris.Client.Data.Cache;
using Kris.Client.Utility;
using Kris.Common.Enums;
namespace Kris.Client.Components.Map;

public partial class KrisPin : StackLayout
{
    private readonly ISymbolImageCache _symbolImageCache;
    private readonly ISymbolImageComposer _symbolImageComposer;

    public string Label { get { return _label; } }
    public string ImagePath { get { return _imagePath; } }
    public ImageSource Image { get { return _image; } }

	private string _label;
	private string _imagePath;
	private ImageSource _image;

    public KrisPin(string label, string imagePath, bool generated)
	{
        _symbolImageCache = ServiceHelper.GetService<ISymbolImageCache>();
        _symbolImageComposer = ServiceHelper.GetService<ISymbolImageComposer>();

        _label = label;
        _imagePath = imagePath;

        if (generated)
        {
            //_image = _symbolImageCache.LoadImageSource(imagePath);

            //_image = ImageSource.FromStream(() => _symbolImageCache.LoadStream(imagePath));

            //MemoryStream stream = new MemoryStream();
            //var fileStream = File.OpenRead(GetFilePath(imagePath));
            //fileStream.CopyTo(stream);
            ////fileStream.Close();
            //stream.Seek(0, SeekOrigin.Begin);
            //_image = ImageSource.FromStream(() => stream);

            //_image = ImageSource.FromFile(GetFilePath(imagePath));

            //_image = _imagePath;

            //_image = ImageSource.FromStream(() => _symbolImageComposer.ComposeMapPointSymbol(MapPointSymbolShape.Circle, MapPointSymbolColor.Red, MapPointSymbolSign.Plus));
        }
        else
        {
            _image = ImageSource.FromFile(imagePath);
        }

        BindingContext = this;
        InitializeComponent();
    }

    private string GetFilePath(string fileName) => Path.Combine(FileSystem.Current.CacheDirectory, fileName);
}
