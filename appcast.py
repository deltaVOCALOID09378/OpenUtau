# Version 0.1
import argparse
from datetime import datetime, timezone
from pathlib import Path

def generate_appcast(version: str, os_name: str, rid: str, file_name: str) -> None:
    """
    Generates the Appcast XML file for OpenUtau updates.
    """
    # Generate timezone-aware datetime for standard XML pubDate formatting
    pub_date = datetime.now(timezone.utc).astimezone().strftime("%a, %d %b %Y %H:%M:%S %z")
    
    # Utilizing f-strings for cleaner and more readable XML construction
    xml_content = f"""<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0" xmlns:sparkle="http://www.andymatuschak.org/xml-namespaces/sparkle">
<channel>
    <title>OpenUtau</title>
    <language>en</language>
    <item>
        <title>OpenUtau {version}</title>
        <pubDate>{pub_date}</pubDate>
        <enclosure url="https://github.com/stakira/OpenUtau/releases/download/{version}/{file_name}"
                   sparkle:version="{version}"
                   sparkle:shortVersionString="{version}"
                   sparkle:os="{os_name}"
                   type="application/octet-stream"
                   sparkle:signature="" />
    </item>
</channel>
</rss>"""

    output_filename = f"appcast.{rid}.xml"
    output_path = Path(output_filename)
    
    try:
        output_path.write_text(xml_content, encoding='utf-8')
        print(f"[Success] Generated {output_filename} successfully.")
    except Exception as e:
        print(f"[Error] Failed to write {output_filename}: {e}")

def main() -> None:
    parser = argparse.ArgumentParser(description='Writes Appcast XML file for OpenUtau.')
    parser.add_argument('-v', '--version', help='Version number', required=True)
    parser.add_argument('-o', '--os', help='OS name', required=True)
    parser.add_argument('-r', '--rid', help='RID', required=True)
    parser.add_argument('-f', '--file', help='File name', required=True)
    
    args = parser.parse_args()
    
    generate_appcast(
        version=args.version,
        os_name=args.os,
        rid=args.rid,
        file_name=args.file
    )

if __name__ == '__main__':
    main()
