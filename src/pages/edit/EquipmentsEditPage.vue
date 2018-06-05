<template>
  <div>
      <grid-component :editedItem="editedItem"
                      @dialog="showModal">
      </grid-component>

    <v-dialog :edit="edit" :dialog="true" v-model= "dialog" slot="activator"
              max-width="500px">
      <v-card>
          <v-card-title>
          <span class="headline">
            {{ formTitle }}
          </span>
        </v-card-title>

        <!-- <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md4 v-for="(header, i) in headers" :key="i">
                <v-text-field v-model="edit[header.value]" label="edit[header.text]"></v-text-field>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text> -->

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" flat @click.native="dialog=false">Cancel</v-btn>
          <v-btn color="blue darken-1" flat @click.native="save">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  props: {
    visible: {
      type: Boolean,
      required: false
    },

    edit: {
      type: Array,
      required: false
    }
  },
    data() {
      return {
        editedIndex: -1,
        dialog: false
      };
    },
    computed: {
      editedItem() {
        return [
          {
            name: '',
            type: '',
            portable: '',
            description: ''
          }
        ];
      },
      formTitle() {
        return this.editedIndex === -1 ? "Yeni Ekleme" : "Düzenleme";
      }
    },

  // watch: {
  //   dialog (val) {
  //     val || this.close()
  //   }
  // },

    methods: {
      // close() {
      //   this.dialog = false;

      //   setTimeout(() => {
      //     this.editedItem = Object.assign({}, this.defaultItem);
      //     this.editedIndex = -1;
      //   }, 300);
      // },

      // save() {
      //   if (this.editedIndex > -1) {
      //     Object.assign(this.items[this.editedIndex], this.editedItem);
      //   } else {
      //     this.items.push(this.editedItem);
      //   }
      //   this.close();
      // }
      showModal(payload){debugger
        this.dialog = payload;
      }
    },
}

</script>
