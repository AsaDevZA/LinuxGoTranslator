package main

import (
	"fmt"
	"log"

	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/container"
	"fyne.io/fyne/v2/widget"
	"github.com/AsaDevZA/LinuxGoTranslator/internal/license"
)

func main() {
	myApp := app.New()
	myWindow := myApp.NewWindow("License Generator")
	myWindow.Resize(fyne.NewSize(600, 500))

	fingerprintEntry := widget.NewEntry()
	fingerprintEntry.SetPlaceHolder("Enter machine fingerprint (MAC address)")

	storeTypeSelect := widget.NewSelect([]string{"Pick n Pay", "Spar", "Arch Software"}, nil)
	storeTypeSelect.SetSelected("Pick n Pay")

	posChannelsEntry := widget.NewEntry()
	posChannelsEntry.SetPlaceHolder("Number of POS channels")
	posChannelsEntry.SetText("10")

	scaleChannelsEntry := widget.NewEntry()
	scaleChannelsEntry.SetPlaceHolder("Number of Scale channels")
	scaleChannelsEntry.SetText("64")

	lprChannelsEntry := widget.NewEntry()
	lprChannelsEntry.SetPlaceHolder("Number of LPR channels")
	lprChannelsEntry.SetText("0")

	advancedReportingCheck := widget.NewCheck("Advanced Reporting", nil)
	cloudStorageCheck := widget.NewCheck("Cloud Storage", nil)

	unlockKeyEntry := widget.NewMultiLineEntry()
	unlockKeyEntry.SetPlaceHolder("Generated unlock key will appear here")
	unlockKeyEntry.Wrapping = fyne.TextWrapWord

	generateBtn := widget.NewButton("Generate License Key", func() {
		fingerprint := fingerprintEntry.Text
		if fingerprint == "" {
			unlockKeyEntry.SetText("Error: Fingerprint is required")
			return
		}

		var storeType license.StoreType
		switch storeTypeSelect.Selected {
		case "Pick n Pay":
			storeType = license.StoreTypePickNPay
		case "Spar":
			storeType = license.StoreTypeSpar
		case "Arch Software":
			storeType = license.StoreTypeArch
		default:
			unlockKeyEntry.SetText("Error: Invalid store type")
			return
		}

		posChannels := 0
		fmt.Sscanf(posChannelsEntry.Text, "%d", &posChannels)

		scaleChannels := 0
		fmt.Sscanf(scaleChannelsEntry.Text, "%d", &scaleChannels)

		lprChannels := 0
		fmt.Sscanf(lprChannelsEntry.Text, "%d", &lprChannels)

		licData := &license.LicenseData{
			StoreType:         storeType,
			POSChannels:       posChannels,
			ScaleChannels:     scaleChannels,
			LPRChannels:       lprChannels,
			AdvancedReporting: advancedReportingCheck.Checked,
			CloudStorage:      cloudStorageCheck.Checked,
		}

		unlockKey, err := license.EncryptLicense(licData, fingerprint)
		if err != nil {
			unlockKeyEntry.SetText(fmt.Sprintf("Error: %v", err))
			return
		}

		unlockKeyEntry.SetText(unlockKey)
		log.Printf("Generated license for %s: Store=%s, POS=%d, Scale=%d, LPR=%d, AdvRep=%v, Cloud=%v",
			fingerprint, storeTypeSelect.Selected, posChannels, scaleChannels, lprChannels,
			advancedReportingCheck.Checked, cloudStorageCheck.Checked)
	})

	copyBtn := widget.NewButton("Copy to Clipboard", func() {
		myWindow.Clipboard().SetContent(unlockKeyEntry.Text)
	})

	content := container.NewVBox(
		widget.NewLabel("License Generator"),
		widget.NewSeparator(),
		widget.NewLabel("Machine Fingerprint:"),
		fingerprintEntry,
		widget.NewLabel("Store Type:"),
		storeTypeSelect,
		widget.NewLabel("POS Channels:"),
		posChannelsEntry,
		widget.NewLabel("Scale Channels:"),
		scaleChannelsEntry,
		widget.NewLabel("LPR Channels:"),
		lprChannelsEntry,
		advancedReportingCheck,
		cloudStorageCheck,
		widget.NewSeparator(),
		generateBtn,
		widget.NewLabel("Unlock Key:"),
		unlockKeyEntry,
		copyBtn,
	)

	myWindow.SetContent(container.NewScroll(content))
	myWindow.ShowAndRun()
}
