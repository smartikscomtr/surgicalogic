<template>
    <div>
      <v-dialog v-model="showModal"
                slot="activator"
                :editedItem="editedItem"
                max-width="500px">
                <!-- :headers="headers" -->
        <v-card>
            <v-card-title>
            <span class="headline">
              {{ formTitle }}
            </span>
          </v-card-title>

          <v-card-text >
            <v-container grid-list-md>
              <v-layout wrap>
                  <v-text-field v-model="editedItem['name']" label=""></v-text-field>
                <!-- <v-flex xs12 sm6 md4 v-for="(header, i) in headers" :key="i">
                </v-flex> -->
              </v-layout>
            </v-container>
          </v-card-text>

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

    editedItem: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    }
  },
    data() {
      return {
        editedIndex: -1
      };
    },

    computed: {
      formTitle() {
        return this.editedIndex === -1 ? "Düzenleme" : "Düzenleme";
      },

      showModal: {
        get() {
          return this.visible;
        },
        set (value) {
          if (!value) {
            this.$emit('close')
          }
        }
      }
    },

    methods: {
    }
}

</script>
