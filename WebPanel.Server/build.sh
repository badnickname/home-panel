dotnet publish -c Release -o ../out --self-contained -r win-x64 -p:PublishSingleFile=true
mkdir ../out/wwwroot
cd ../WebPanel.UI/web-panel
npm run build -- --output-path ../../out/wwwroot/
cp -r ../../out/wwwroot/browser/* ../../out/wwwroot/
rm -rf ../../out/wwwroot/browser